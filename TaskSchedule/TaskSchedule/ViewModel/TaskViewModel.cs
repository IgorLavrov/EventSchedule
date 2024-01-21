using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Threading.Tasks;
using TaskSchedule.Models;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TaskSchedule.ViewModel
{
    public class TaskViewModel:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool isViewDetail = false;
        public bool IsViewDetail
        {
            get { return isViewDetail; }
            set
            {
                isViewDetail = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsViewDetail"));
            }
        }

        private string typeCommand = string.Empty;
        public string TypeCommand
        {
            get { return typeCommand; }
            set
            {
                typeCommand = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TypeCommand"));
            }
        }
        //----------------------------------------
        private string taskName;
        public string TaskName
        {
            get { return taskName; }
            set
            {
                taskName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TaskName"));
            }
        }

        private string description;
        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Description"));
            }
        }
        private string address;
        public string Address
        {
            get { return address; }
            set
            {
                address = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Address"));
            }
        }

        private DateTime dueDate = DateTime.Now;
        public DateTime DueDate
        {
            get { return dueDate; }
            set
            {

                dueDate = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DueDate"));
            }
        }

        private List<TaskModel> taskList;
        public List<TaskModel> TaskList
        {
            get { return taskList; }
            set
            {


                taskList = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TaskList"));


            }
        }

        private TaskModel selectedTask;
        public TaskModel SelectedTask
        {
            get { return selectedTask; }
            set
            {
                selectedTask = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedTask"));
            }
        }
        //------------
        public Command cmdProcessTask { get; set; }
        public Command cmdCancelTask { get; set; }
        //-----------
        public Command cmdAddaTask { get; set; }
        public Command cmdDeleteaTask { get; set; }
        public Command cmdUpdateaTask { get; set; }
        public Command cmdMapTask { get; set; }

        public TaskViewModel()
        {
            cmdProcessTask = new Command(ProcessTask);
            cmdCancelTask = new Command(CancelTask);
            //------------------
            cmdAddaTask = new Command(AddaTask);
            cmdDeleteaTask = new Command(DeleteaTask);
            cmdUpdateaTask = new Command(UpdateaTask);
            getTask();
            cmdMapTask = new Command(MapTask);
            
        }

        private void UpdateaTask(object obj)
        {
            IsViewDetail = true;
            TypeCommand = "Update";
            if (selectedTask != null)
            {
                TaskName = selectedTask.TaskName;
                Description = selectedTask.Description;
                Address= selectedTask.Address;
                DueDate = selectedTask.DueDate;

            }
            else
            {
                IsViewDetail = false;
                typeCommand = string.Empty;
            }
        }

        private void DeleteaTask(object obj)
        {
            IsViewDetail = true;
            TypeCommand = "Delete";
            if (selectedTask != null)
            {
                TaskName = selectedTask.TaskName;
                Description = selectedTask.Description;
                Address = selectedTask.Address;
                DueDate = selectedTask.DueDate;
            }
            else
            {
                IsViewDetail = false;
                typeCommand = string.Empty;
            }

        }

        private void AddaTask(object obj)
        {
            IsViewDetail = true;
            TypeCommand = "Add";
            TaskName = Description = Address = string.Empty;
        }

        private void CancelTask(object obj)
        {
            IsViewDetail = false;
            typeCommand = string.Empty;
        }

        private async void ProcessTask(object obj)
        {

            if (TypeCommand == "Add")
            {
                var r = await App.TaskDatabase.SaveTaskAsync(new TaskModel
                {
                    TaskName = TaskName,
                    Description = Description,
                    Address = Address,
                    DueDate = DueDate
                });

            }
            else if (TypeCommand == "Update")
            {
                selectedTask.TaskName = TaskName;
                selectedTask.Description = Description;
                SelectedTask.Address = Address;
                selectedTask.DueDate = DueDate;
                await App.TaskDatabase.UpdateTaskAsync(SelectedTask);

            }
            else if (TypeCommand == "Delete")
            {
                await App.TaskDatabase.DeleteTaskAsync(SelectedTask);
            }
            IsViewDetail = false;
            typeCommand = string.Empty;
            selectedTask = null;
            getTask();
        }
        public async void getTask()
        {
            TaskList = await App.TaskDatabase.GetTaskAsync();
        }


        //public ICommand cmdMapTask
        //{
        //    get
        //    {
        //        if (cmdMapTask1 == null)
        //        {
        //            cmdMapTask1 = new Command(PerformcmdMapTask);
        //        }

        //        return cmdMapTask1;
        //    }
        //}

        public async void MapTask(Object obj)
        {
            if (!string.IsNullOrWhiteSpace(SelectedTask.Address))
            {
                try
                {
                    var location = await Geocoding.GetLocationsAsync(SelectedTask.Address);

                    if (location != null && location.Any())
                    {
                        var firstLocation = location.First();
                        var options = new MapLaunchOptions { Name = selectedTask.TaskName };

                        await Map.OpenAsync(firstLocation, options);
                    }
                    else
                    {
                        await DisplayAlert("Error", "Unable to find the location on the map.", "OK");
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", $"Failed to find the location on the map. {ex.Message}", "OK");
                }
            }
            else
            {
                await DisplayAlert("Error", "Address is not provided.", "OK");
            }
        }

        private async Task DisplayAlert(string title, string message, string cancel)
        {
            await Application.Current.MainPage.DisplayAlert(title, message, cancel);
        }

    }
}
