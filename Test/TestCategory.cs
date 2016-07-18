using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace ToDoList
{
  public class CategoryTest : IDisposable
  {
    public CategoryTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=todo_test;Integrated Security=SSPI;";
    }
    [Fact]
    public void Test_CategoriesEmptyAtFirst()
    {
      int result = Category.GetAll().Count;
      Assert.Equal(0, result);
    }
    [Fact]
    public void Test_Equal_ReturnsTrueForSameName()
    {
      Category firstCategory = new Category("Household chores");
      Category secondCategory = new Category("Household chores");
      Assert.Equal(firstCategory, secondCategory);
    }
    [Fact]
    public void Test_Save_SavesCategoryToDatabase()
    {
      Category testCategory = new Category("Household chores");
      testCategory.Save();
      List<Category> result = Category.GetAll();
      List<Category> testList = new List<Category>{testCategory};
      Assert.Equal(testList, result);
    }
    [Fact]
    public void Test_Save_AssignsIdToCategoryObject()
    {
      Category testCategory = new Category("Household chores");
      testCategory.Save();
      Category savedCategory = Category.GetAll()[0];
      int result = savedCategory.GetId();
      int testId = testCategory.GetId();
      Assert.Equal (testId, result);
    }
    [Fact]
    public void Test_Find_FindsCategoryInDatabase()
    {
      Category testCategory = new Category("Household chores");
      testCategory.Save();
      Category foundCategory = Category.Find(testCategory.GetId());
      Assert.Equal(testCategory, foundCategory);
    }
    [Fact]
    public void Test_AddTask_AddsTaskToCategory()
    {
      //Arrange
      Category testCategory = new Category("Household chores");
      testCategory.Save();

      Task testTask = new Task("Mow the lawn", new DateTime(2016, 5, 4));
      testTask.Save();

      Task testTask2 = new Task("Water the garden", new DateTime(2016, 5, 4));
      testTask2.Save();

      //Act
      testCategory.AddTask(testTask);
      testCategory.AddTask(testTask2);
      List<Task> result = testCategory.GetTasks();
      List<Task> testList = new List<Task>{testTask, testTask2};
      Console.WriteLine(result);

      //Assert
      Assert.Equal(testList, result);
    }
    [Fact]
    public void Test_Delete_DeletesCategoryAssociationsFromDatabase()
    {
      //Arrange
      Task testTask = new Task("Mow the lawn", new DateTime(2016, 5, 4));
      testTask.Save();

      string testName = "Home stuff";
      Category testCategory = new Category(testName);
      testCategory.Save();

      //Act
      testCategory.AddTask(testTask);
      testCategory.Delete();

      List<Category> resultTaskCategories = testTask.GetCategories();
      List<Category> testTaskCategories = new List<Category> {};

      //Assert
      Assert.Equal(testTaskCategories, resultTaskCategories);
    }
    public void Dispose()
    {
      Task.DeleteAll();
      Category.DeleteAll();
    }
  }
}
