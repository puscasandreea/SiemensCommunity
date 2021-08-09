﻿using Data.Implementations;
using Data.Contracts;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Microsoft.EntityFrameworkCore;

namespace Data.Tests
{
    public class DepartmentsTests
    {
        private DepartmentRepository repository;
        private Mock<IDepartmentRepository> departmentRepositoryMock;
        private List<Department> departments;
        DbContextOptions<ProjectDbContext> options = new DbContextOptionsBuilder<ProjectDbContext>()
                                             .UseInMemoryDatabase(databaseName: "SiemensCommunityTests")
                                             .Options;
        private ProjectDbContext dbContext; 

        [SetUp]
        public void Setup()
        {
            using (var context = new ProjectDbContext(options))
            {
                context.Departments.Add(new Department { Id = 1, Name = "Department 1"});
                context.Departments.Add(new Department { Id = 2, Name = "Department 2" });
                context.Departments.Add(new Department { Id = 3, Name = "Department 3"});
                context.SaveChanges();
            }
            departmentRepositoryMock = new Mock<IDepartmentRepository>();
            dbContext = new ProjectDbContext(options);
            repository = new DepartmentRepository(dbContext);
        }

        [Test]
        public async Task GetDepartment_ShouldReturnListOfDepartments()
        {
            departmentRepositoryMock.Setup(p => p.GetAsync()).Returns(Task.FromResult(departments.AsEnumerable()));

            var result = await repository.GetAsync();

            Assert.IsInstanceOf<IEnumerable<Department>>(result);
            Assert.AreEqual(result.Count(), 3);
        }
    }
}