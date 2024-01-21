using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TaskSchedule.Models;

namespace TaskSchedule.Services
{
    public  class TaskDatabaseService
    {
        readonly SQLite.SQLiteAsyncConnection _database;

            public TaskDatabaseService(string dbPath)
            {
                _database = new SQLiteAsyncConnection(dbPath);
                _database.CreateTableAsync<TaskModel>().Wait();
            }

            public Task<List<TaskModel>> GetTaskAsync()
            {
                return _database.Table<TaskModel>().OrderByDescending(x => x.DueDate).ToListAsync();
            }

            public Task<int> SaveTaskAsync(TaskModel taskModel)
            {
                return _database.InsertAsync(taskModel);

            }
            public async Task DeleteTaskAsync(TaskModel taskModel)
            {
                await _database.DeleteAsync(taskModel);
            }
            public async Task<int> UpdateTaskAsync(TaskModel taskModel)
            {
                return await _database.UpdateAsync(taskModel);
            }
            public async Task<TaskModel> GetaTask(int id)

            {
            return await _database.GetAsync<TaskModel>(id);
            //return await _database.FindAsync<TaskModel>(id);
        }

        }
    
}
