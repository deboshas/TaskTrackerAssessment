using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using TaskTracker.Domain.Task;
using TaskTracker.Infrastructure.Persistance;
using TaskTracker.Infrastructure.Repositories;

namespace TaskTracker.Infrastructure.Tests
{
    public class TaskTrackerRepositoryTests
    {
        // Replace with your actual repository and setup
        private TaskTrackerRepository CreateRepository()
        {
            // TODO: Setup in-memory DB or mocks as needed

            var options = new DbContextOptionsBuilder<TaskTrackerDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString())
               .Options;

            var dbContext = new TaskTrackerDbContext(options);
            var logger = new Mock<ILogger<TaskTrackerRepository>>().Object;
            return new TaskTrackerRepository(dbContext, logger);
        }

        [Fact]
        public async Task QueryAsync_ReturnsQueryableTasks()
        {
            var repo = CreateRepository();

            var result = await repo.QueryAsync();

            Assert.NotNull(result);
            Assert.IsAssignableFrom<IQueryable<TaskItem>>(result);
        }

        [Fact]
        public void Add_AddsSingleTask()
        {
            var repo = CreateRepository();
            var task = new TaskItem { Id = Guid.NewGuid(), Title = "Test", UserId = "user" };

            repo.Add(task);

            // TODO: Assert the task was added (depends on your implementation)
        }

        [Fact]
        public void Add_AddsMultipleTasks()
        {
            var repo = CreateRepository();
            var tasks = new List<TaskItem>
            {
                new TaskItem { Id = Guid.NewGuid(), Title = "Task1", UserId = "user1" },
                new TaskItem { Id = Guid.NewGuid(), Title = "Task2", UserId = "user2" }
            };

            repo.Add(tasks);

            // TODO: Assert the tasks were added
        }

        [Fact]
        public void Update_UpdatesSingleTask()
        {
            var repo = CreateRepository();
            var task = new TaskItem { Id = Guid.NewGuid(), Title = "Test", UserId = "user" };

            repo.Update(task);

            // TODO: Assert the task was updated
        }

        [Fact]
        public void Update_UpdatesMultipleTasks()
        {
            var repo = CreateRepository();
            var tasks = new List<TaskItem>
            {
                new TaskItem { Id = Guid.NewGuid(), Title = "Task1", UserId = "user1" },
                new TaskItem { Id = Guid.NewGuid(), Title = "Task2", UserId = "user2" }
            };

            repo.Update(tasks);

            // TODO: Assert the tasks were updated
        }

        [Fact]
        public void Delete_DeletesSingleTask()
        {
            var repo = CreateRepository();
            var task = new TaskItem { Id = Guid.NewGuid(), Title = "Test", UserId = "user" };

            repo.Delete(task);

            // TODO: Assert the task was deleted
        }

        [Fact]
        public void Delete_DeletesMultipleTasks()
        {
            var repo = CreateRepository();
            var tasks = new List<TaskItem>
            {
                new TaskItem { Id = Guid.NewGuid(), Title = "Task1", UserId = "user1" },
                new TaskItem { Id = Guid.NewGuid(), Title = "Task2", UserId = "user2" }
            };

            repo.Delete(tasks);

            // TODO: Assert the tasks were deleted
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsTask_WhenExists()
        {
            var repo = CreateRepository();
            var id = Guid.NewGuid();

            var result = await repo.GetByIdAsync(id, CancellationToken.None);

            // TODO: Assert result is correct (depends on your implementation)
        }
    }
}
