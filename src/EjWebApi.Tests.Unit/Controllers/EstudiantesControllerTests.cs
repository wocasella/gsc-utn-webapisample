using EjWebApi.Controllers;
using EjWebApi.DataAccess;
using EjWebApi.Domain;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace EjWebApi.Tests.Unit.Controllers
{
    public class EstudiantesControllerTests : IDisposable
    {
        private readonly SqliteConnection connection = new("Filename=:memory:");
        private readonly AcademiaDbContext context;
        private readonly EstudiantesController sut;

        public EstudiantesControllerTests()
        {
            this.connection.Open();

            var options = new DbContextOptionsBuilder()
                .UseSqlite(this.connection)
                .Options;

            this.context = new AcademiaDbContext(options);

            this.sut = new EstudiantesController(this.context);
        }

        public void Dispose() => this.connection.Dispose();

        public class TheMethod_GetAll : EstudiantesControllerTests
        {
            [Fact]
            public async Task Should_return_all_three_init_students()
            {
                // arrange
                await this.InitAsync();

                // act
                ActionResult<IEnumerable<Estudiante>> actual = await this.sut.GetAll();

                // assert
                actual.Value.Should().HaveCount(3);
            }
        }

        public class TheMethod_GetById : EstudiantesControllerTests
        {
            [Fact]
            public async Task Should_return_student_with_Id_equals_two()
            {
                // arrange
                await this.InitAsync();

                // act
                ActionResult<Estudiante?> actual = await this.sut.GetById(2);

                // assert
                actual.Value.Should().NotBeNull();

                Estudiante studentTwo = actual.Value!;

                using (new AssertionScope())
                {
                    studentTwo.Id.Should().Be(2);
                    studentTwo.Apellido.Should().Be("Gomez");
                    studentTwo.Nombre.Should().Be("Lucía");
                    studentTwo.FechaNacimiento.Should().HaveYear(1993).And.HaveMonth(9).And.HaveDay(12);
                    studentTwo.Legajo.Should().Be(98765);
                }
            }

            [Fact]
            public async Task Should_return_NotFound_when_student_does_not_exists()
            {
                // arrange
                await this.InitAsync();

                // act
                ActionResult<Estudiante?> actual = await this.sut.GetById(100);

                // assert
                actual.Result.Should().BeOfType<NotFoundResult>();
                actual.Value.Should().BeNull();
            }
        }

        public class TheMethod_Delete : EstudiantesControllerTests
        {
            [Fact]
            public async Task Should_remove_student_with_Id_equals_one()
            {
                // arrange
                await this.InitAsync();

                // act
                ActionResult actual = await this.sut.Delete(1);

                // assert
                actual.Should().BeOfType<NoContentResult>();

                var studentOne = await this.context.FindAsync<Estudiante>(1);
                studentOne.Should().BeNull();
            }
        }


        private async Task InitAsync()
        {
            await this.context.AddRangeAsync(
                new Estudiante()
                {
                    Apellido = "Perez",
                    Nombre = "Juan",
                    FechaNacimiento = new DateOnly(1995, 6, 2),
                    Legajo = 12345
                },
                new Estudiante()
                {
                    Apellido = "Gomez",
                    Nombre = "Lucía",
                    FechaNacimiento = new DateOnly(1993, 9, 12),
                    Legajo = 98765
                },
                new Estudiante()
                {
                    Apellido = "Ruiz",
                    Nombre = "Julia",
                    FechaNacimiento = new DateOnly(2001, 2, 27),
                    Legajo = 86420
                });

            await this.context.SaveChangesAsync();
        }
    }
}
