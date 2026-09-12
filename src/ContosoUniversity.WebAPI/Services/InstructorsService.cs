using ContosoUniversity.WebAPI.Data;
using ContosoUniversity.WebAPI.Entities;
using ContosoUniversity.WebAPI.Exceptions;
using ContosoUniversity.WebAPI.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.WebAPI.Services
{
    public class InstructorsService(SchoolContext context)
    {
        public async Task<PaginationResult<InstructorListVM>> GetPagedAsync(int pageIndex, int pageSize)
        {
            return await PaginationResult<InstructorListVM>.Create(pageIndex, pageSize, context.Instructors
                .OrderBy(i => i.ID)
                .Select(i => new InstructorListVM
                {
                    ID = i.ID,
                    LastName = i.LastName,
                    FirstName = i.FirstMidName,
                    HireDate = i.HireDate,
                    Office = i.OfficeAssignment != null ? i.OfficeAssignment.Location : "No office",
                    CourseBriefs = i.Courses
                        .Select(c => new CourseBrief
                        {
                            CourseID = c.CourseID,
                            Title = c.Title,
                        }).ToList()
                }));
        }

        public async Task<List<CourseListVM>> GetCoursesByInstructorIDAsync(int insructorID)
        {
            return await context.Instructors
                 .Where(i => i.ID == insructorID)
                 .SelectMany(i => i.Courses)
                 .Select(c => new CourseListVM
                 {
                     CourseID = c.CourseID,
                     Title = c.Title,
                     Credits = c.Credits,
                     Department = c.Department.Name
                 })
                 .ToListAsync();
        }

        public async Task<InstructorDetailVM> GetByIDAsync(int id)
        {
            var instructor = await context.Instructors
                 .Select(i => new InstructorDetailVM
                 {
                     ID = i.ID,
                     LastName = i.LastName,
                     FirstName = i.FirstMidName,
                     HireDate = i.HireDate,
                     Office = i.OfficeAssignment != null ? i.OfficeAssignment.Location : "No office",
                     CourseBriefs = i.Courses
                        .Select(c => new CourseBrief
                        {
                            CourseID = c.CourseID,
                            Title = c.Title,
                        }).ToList()
                 })
                 .FirstOrDefaultAsync(i => i.ID == id)
                 ?? throw new NotFoundException($"Instructor with ID {id} not found.");

            return instructor;
        }

        public async Task<InstructorDetailVM> CreateAsync(CreateInstructorVM instructorVM)
        {
            var instructor = new Instructor
            {
                LastName = instructorVM.LastName,
                FirstMidName = instructorVM.FirstName,
                HireDate = instructorVM.HireDate
            };

            // Assign office location if provided
            if (!string.IsNullOrEmpty(instructorVM.Office))
            {
                instructor.OfficeAssignment = new OfficeAssignment
                {
                    InstructorID = instructor.ID,
                    Location = instructorVM.Office
                };
            }

            // Associate courses with the instructor if any course IDs are provided
            if (instructorVM.CourseIDs.Count > 0)
            {
                await context.Courses.LoadAsync();
                instructor.Courses = [];

                // Loop through the provided course IDs and associate them with the instructor
                foreach (var courseID in instructorVM.CourseIDs)
                {
                    var course = await context.Courses.FindAsync(courseID);
                    if (course != null)
                    {
                        instructor.Courses.Add(course);
                    }
                    else
                    {
                        throw new NotFoundException($"Course with ID {courseID} not found.");
                    }
                }
            }

            context.Add(instructor);
            await context.SaveChangesAsync();

            return new InstructorDetailVM
            {
                ID = instructor.ID,
                LastName = instructor.LastName,
                FirstName = instructor.FirstMidName,
                HireDate = instructor.HireDate,
                Office = instructor.OfficeAssignment?.Location ?? "No office",
                CourseBriefs = [.. instructor.Courses
                     .Select(c => new CourseBrief
                     {
                         CourseID = c.CourseID,
                         Title = c.Title,
                     })]
            };
        }

        public async Task UpdateAsync(int id, UpdateInstructorVM instructorVM)
        {
            var instructor = await GetInstructorOrThrowAsync(id);

            instructor.LastName = instructorVM.LastName;
            instructor.FirstMidName = instructorVM.FirstName;
            instructor.HireDate = instructorVM.HireDate;

            // Update office assignment
            if (instructor.OfficeAssignment != null)
            {
                instructor.OfficeAssignment.Location = instructorVM.Office;
            }
            else
            {
                if (!string.IsNullOrEmpty(instructorVM.Office))
                {
                    instructor.OfficeAssignment = new OfficeAssignment
                    {
                        InstructorID = instructor.ID,
                        Location = instructorVM.Office
                    };
                }
            }

            // Associate courses with the instructor if any course IDs are provided
            instructor.Courses = [];

            if (instructorVM.CourseIDs.Count > 0)
            {
                await context.Courses.LoadAsync();

                // Loop through the provided course IDs and associate them with the instructor
                foreach (var courseID in instructorVM.CourseIDs)
                {
                    var course = await context.Courses.FindAsync(courseID);
                    if (course != null)
                    {
                        instructor.Courses.Add(course);
                    }
                    else
                    {
                        throw new NotFoundException($"Course with ID {courseID} not found.");
                    }
                }
            }

            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var instructor = await GetInstructorOrThrowAsync(id);
            context.Instructors.Remove(instructor);
            await context.SaveChangesAsync();
        }

        private async Task<Instructor> GetInstructorOrThrowAsync(int id)
        {
            var instructor = await context.Instructors
                .Include(i => i.OfficeAssignment)
                .Include(i => i.Courses)
                .FirstOrDefaultAsync(i => i.ID == id)
                ?? throw new NotFoundException($"Instructor with ID {id} not found.");
            return instructor;
        }
    }
}
