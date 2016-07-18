using System;
using System.Collections.Generic;
using Nancy;
using Nancy.ViewEngines.Razor;

namespace ToDoList
{
  public class HomeModule : NancyModule
  {
    public HomeModule()
    {
      Get ["/"]= _ =>{
        return View ["index.cshtml"];
      };
      Get ["/tasks"]= _ => {
        List<Task> AllTasks = Task.GetAll();
        return View["tasks.cshtml", AllTasks];
      };
      Get ["/categories"]= _ =>{
        List<Category> allCategories = Category.GetAll();
        return View ["categories.cshtml", allCategories];
      };
      Get ["tasks/new"]= _ =>{
        return View ["task_form.cshtml"];
      };
      Post ["tasks/new"]= _ =>{
        Task newTask = new Task(Request.Form["task-description"],Request.Form["task-complete"]);
        newTask.Save();
        List<Task> AllTasks = Task.GetAll();
        return View ["tasks.cshtml", AllTasks];
      };
      Get ["categories/new"]= _ =>{
        return View ["categories_form.cshtml"];
      };
      Post ["categories/new"]= _ =>{
        Category newCategory = new Category(Request.Form["category-name"]);
        newCategory.Save();
        List<Category> allCategories = Category.GetAll();
        return View ["categories.cshtml", allCategories];
      };
      Get["tasks/{id}"] = parameters => {
        Dictionary<string, object> model = new Dictionary<string, object>();
        Task SelectedTask = Task.Find(parameters.id);
        List<Category> TaskCategories = SelectedTask.GetCategories();
        List<Category> AllCategories = Category.GetAll();
        model.Add("task", SelectedTask);
        model.Add("taskCategories", TaskCategories);
        model.Add("allCategories", AllCategories);
        return View["task.cshtml", model];
      };

      Get["categories/{id}"] = parameters => {
        Dictionary<string, object> model = new Dictionary<string, object>();
        Category SelectedCategory = Category.Find(parameters.id);
        List<Task> CategoryTasks = SelectedCategory.GetTasks();
        List<Task> AllTasks = Task.GetAll();
        model.Add("category", SelectedCategory);
        model.Add("categoryTasks", CategoryTasks);
        model.Add("allTasks", AllTasks);
        return View["category.cshtml", model];
      };
      Post["task/add_category"] = _ => {
        Category category = Category.Find(Request.Form["category-id"]);
        Task task = Task.Find(Request.Form["task-id"]);
        task.AddCategory(category);
        return View["success.cshtml"];
      };
      Post["category/add_task"] = _ => {
        Category category = Category.Find(Request.Form["category-id"]);
        Task task = Task.Find(Request.Form["task-id"]);
        category.AddTask(task);
        return View["success.cshtml"];
      };
      Post ["/taskComplete"]= _ =>{
        // taskName = (Request.Form["task-id"]);
        // taskBool =(Request.Form["task-complete"]);
        // Task task = new Task (taskName, taskBool);
        // task.Save();
        Task task = Task.Find(Request.Form["GetId"]);
        task.Update(Request.Form["task-complete"]);
        return View["success.cshtml"];
      };
    }
  }
}
