using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


public interface IRepository
{
    List<IUser> GetDepartmentUsers();
    List<IUser> GetAllUsers();
    IUser GetUser(string username);
    IUser GetUser(int id);
    List<IPublication> GetPublications();
    decimal? GetJobTotal(int JobID);
    System.Data.Linq.DataContext db { get;}
 }

public interface IUser
{
    int ID { get; set; }
    string UserName { get; set; }
    string Email { get; set; }
    string FirstName { get; set; }
    string LastName { get; set; }
    bool Active { get; set; }
}

public interface IPublication
{
    int ID { get; set; }
    string name { get; set; }
    bool active { get; set; }
}

public interface IItem
{
    int ID { get; set; }
    string name { get; set; }
    bool active { get; set; }
}