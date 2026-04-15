using FluentAssertions;
using NPoco;
using SixBeeHealthCare.Tests.Infrastructure;
using SixBeeHealthCare.Web.Models;
using SixBeeHealthCare.Web.Services;

namespace SixBeeHealthCare.Tests.Services
{
    [TestFixture]
    public class AppointmentServiceTests
    {
        private TestDatabaseFixture _fixture = null!;
        private IDatabase _db = null!;
        private AppointmentService _sut = null!;

        [OneTimeSetUp]
        public void CreateDatabase()
        {
            _fixture = new TestDatabaseFixture();
        }

        [OneTimeTearDown]
        public void DropDatabase()
        {
            _fixture.Dispose();
        }

        [SetUp]
        public void BeginTransaction()
        {
            _db = _fixture.OpenDatabase();
            _db.BeginTransaction();
            _sut = new AppointmentService(_db);
        }

        [TearDown]
        public void RollbackTransaction()
        {
            _db.AbortTransaction();
            _db.Dispose();
        }


        private static Appointment MakeAppointment(DateTime? dateTime = null) => new()
        {
            PatientName = "Test Patient",
            AppointmentDateTime = dateTime ?? DateTime.UtcNow.AddDays(1),
            Description = "Routine check-up",
            ContactNumber = "07700 900000",
            EmailAddress = "patient@example.com"
        };

        //Create

        [Test]
        public async Task CreateAsync_ShouldPersistAppointment()
        {
            await _sut.CreateAsync(MakeAppointment());

            var all = await _sut.GetAllOrderedByDateAsync();
            all.Should().HaveCount(1);
        }

        [Test]
        public async Task CreateAsync_ShouldSetCreatedAtToNow()
        {
            var before = DateTime.UtcNow;
            var appt = MakeAppointment();

            await _sut.CreateAsync(appt);

            appt.CreatedAt.Should().BeOnOrAfter(before);
        }

        //GetAll
        [Test]
        public async Task GetAllOrderedByDateAsync_ShouldReturnAppointmentsOrderedByDate()
        {
            await _sut.CreateAsync(MakeAppointment(DateTime.UtcNow.AddDays(5)));
            await _sut.CreateAsync(MakeAppointment(DateTime.UtcNow.AddDays(1)));

            var results = (await _sut.GetAllOrderedByDateAsync()).ToList();

            results[0].AppointmentDateTime.Should().BeBefore(results[1].AppointmentDateTime);
        }

        [Test]
        public async Task GetAllOrderedByDateAsync_ShouldReturnEmpty_WhenNoAppointments()
        {
            var results = await _sut.GetAllOrderedByDateAsync();
            results.Should().BeEmpty();
        }

        //GetById
        [Test]
        public async Task GetByIdAsync_ShouldReturnAppointment_WhenExists()
        {
            var appt = MakeAppointment();
            await _sut.CreateAsync(appt);

            var result = await _sut.GetByIdAsync(appt.Id);

            result.Should().NotBeNull();
            result!.PatientName.Should().Be("Test Patient");
        }

        [Test]
        public async Task GetByIdAsync_ShouldReturnNull_WhenNotFound()
        {
            var result = await _sut.GetByIdAsync(999);
            result.Should().BeNull();
        }

        //Approve
        [Test]
        public async Task ApproveAsync_ShouldSetIsApproved_ToTrue()
        {
            var appt = MakeAppointment();
            await _sut.CreateAsync(appt);

            await _sut.ApproveAsync(appt.Id);

            var updated = await _sut.GetByIdAsync(appt.Id);
            updated!.IsApproved.Should().BeTrue();
        }

        [Test]
        public async Task ApproveAsync_ShouldToggle_WhenCalledTwice()
        {
            var appt = MakeAppointment();
            await _sut.CreateAsync(appt);

            await _sut.ApproveAsync(appt.Id);
            await _sut.ApproveAsync(appt.Id);

            var updated = await _sut.GetByIdAsync(appt.Id);
            updated!.IsApproved.Should().BeFalse();
        }

        //Update
        [Test]
        public async Task UpdateAsync_ShouldPersistChanges()
        {
            var appt = MakeAppointment();
            await _sut.CreateAsync(appt);

            appt.PatientName = "Updated Name";
            await _sut.UpdateAsync(appt);

            var updated = await _sut.GetByIdAsync(appt.Id);
            updated!.PatientName.Should().Be("Updated Name");
        }

        //Delete
        [Test]
        public async Task DeleteAsync_ShouldRemoveAppointment()
        {
            var appt = MakeAppointment();
            await _sut.CreateAsync(appt);

            await _sut.DeleteAsync(appt.Id);

            var result = await _sut.GetByIdAsync(appt.Id);
            result.Should().BeNull();
        }

        [Test]
        public async Task DeleteAsync_ShouldNotThrow_WhenIdNotFound()
        {
            Assert.DoesNotThrowAsync(() => _sut.DeleteAsync(999));
        }
    }
}
