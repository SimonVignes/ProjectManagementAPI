using System;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore.Sqlite;

namespace ProjectManagementAPI.Models;

public class SQLDatabaseContext
{
    private SqliteConnection connection = new SqliteConnection($"Data Source=./projectDB.sqlite;");

    // Method used for creating new tables with information
    // This has to be run manually
    public void CreateProjectTable()
    {

        connection.Open();

        var createTableQuery = @"
                    CREATE TABLE IF NOT EXISTS Project (
                        project_id INTEGER PRIMARY KEY,
                        name VARCHAR(35),
                        description TEXT,
                        project_manager VARCHAR(35)
                    );
                    
                    CREATE TABLE IF NOT EXISTS Epic (
                        epic_id INTEGER PRIMARY KEY,
                        name VARCHAR(35),
                        description TEXT,
                        project_id INTEGER,
                        FOREIGN KEY(project_id) REFERENCES Project(project_id)
                    );
                    
                    CREATE TABLE IF NOT EXISTS SubTask (
                        subtask_id INTEGER PRIMARY KEY,
                        name VARCHAR(35),
                        description TEXT,
                        epic_id INTEGER,
                        FOREIGN KEY(epic_id) REFERENCES Epic(epic_id)
                    );";

        var createTableCmd = new SqliteCommand(createTableQuery, connection);

        createTableCmd.ExecuteNonQuery();


        var insertDataQuery = @"
                    INSERT INTO Project (project_id, name, description, project_manager)
                    VALUES (1, 'Project 1', 'This is the first project', 'Simon Vignes');

                    INSERT INTO Project (project_id, name, description, project_manager)
                    VALUES (2, 'Project 2', 'This is the second project', 'Simon Vignes');

                    INSERT INTO Epic (epic_id, name, description, project_id)
                    VALUES (1, 'Epic 1', 'This is the first epic', 1);
                    
                    INSERT INTO Epic (epic_id, name, description, project_id)
                    VALUES (2, 'Epic 2', 'This is the second epic', 1);
                    
                    INSERT INTO Epic (epic_id, name, description, project_id)
                    VALUES (3, 'Epic 3', 'This is the third epic', 2);

                    INSERT INTO Epic (epic_id, name, description, project_id)
                    VALUES (4, 'Epic 4', 'This is the fourth epic', 2);
                    
                    INSERT INTO SubTask (subtask_id, name, description, epic_id)
                    VALUES (1, 'Subtask 1', 'This is the first subtask', 1);

                    INSERT INTO SubTask (subtask_id, name, description, epic_id)
                    VALUES (2, 'Subtask 2', 'This is the second subtask', 1);

                    INSERT INTO SubTask (subtask_id, name, description, epic_id)
                    VALUES (3, 'Subtask 3', 'This is the third subtask', 2);

                    INSERT INTO SubTask (subtask_id, name, description, epic_id)
                    VALUES (4, 'Subtask 4', 'This is the fourth subtask', 2);

                    INSERT INTO SubTask (subtask_id, name, description, epic_id)
                    VALUES (5, 'Subtask 5', 'This is the fifth subtask', 3);

                    INSERT INTO SubTask (subtask_id, name, description, epic_id)
                    VALUES (6, 'Subtask 6', 'This is the sixth subtask', 3);

                    INSERT INTO SubTask (subtask_id, name, description, epic_id)
                    VALUES (7, 'Subtask 7', 'This is the seventh subtask', 4);

                    INSERT INTO SubTask (subtask_id, name, description, epic_id)
                    VALUES (8, 'Subtask 8', 'This is the eighth subtask', 4);";

        var insertProjectInfoCmd = new SqliteCommand(insertDataQuery, connection);

        insertProjectInfoCmd.ExecuteNonQuery();

        connection.Close();
    }
    



    // Getting all projects
    public List<ProjectItem>? GetProjects()
    {
        connection.Open();

        var query = "SELECT * FROM Project;";
        var command = new SqliteCommand(query, connection);
        var reader = command.ExecuteReader();

        List<ProjectItem> projects = new List<ProjectItem> { };
        while (reader.Read())
        {
            ProjectItem project = ReadProjectItem(reader);
            projects.Add(project);
        }

        connection.Close();

        return projects;

    }

    public ProjectItem? GetProjectById(int id)
    {
        connection.Open();

        var query = $"SELECT * FROM Project WHERE project_id = @id;";
        var command = new SqliteCommand(query, connection);
        command.Parameters.AddWithValue("@id", id);
        var reader = command.ExecuteReader();

        ProjectItem? project = null;
        if (reader.Read())
        {
            project = ReadProjectItem(reader);
        }

        connection.Close();

        return project;
    }


    private ProjectItem ReadProjectItem(SqliteDataReader reader)
    {
        var project = new ProjectItem
        {
            Project_id = reader.GetInt32(0),
            Name = reader.GetString(1),
            Description = reader.GetString(2),
            ProjectManager = reader.GetString(3)
        };
        return project;
    }


    public bool AddProject(ProjectItem project)
    {


        connection.Open();

        var query = "SELECT * FROM Project WHERE project_id = @id;";
        var command = new SqliteCommand(query, connection);
        command.Parameters.AddWithValue("@id", project.Project_id);
        var reader = command.ExecuteReader();

        if (reader.Read())
        {
            return false; // Project already exists, return false
        }


        var insertQuery = "INSERT INTO Project (project_id, name, description, project_manager) VALUES (@Project_id, @Name, @Description, @ProjectManager);";
        var insertCommand = new SqliteCommand(insertQuery, connection);
        insertCommand.Parameters.AddWithValue("@Project_id", project.Project_id);
        insertCommand.Parameters.AddWithValue("@Name", project.Name);
        insertCommand.Parameters.AddWithValue("@Description", project.Description);
        insertCommand.Parameters.AddWithValue("@ProjectManager", project.ProjectManager);
        insertCommand.ExecuteNonQuery();

        connection.Close();

        return true;

    }



    public bool UpdateProject(ProjectItem project)
    {
        connection.Open();

        // Checking if the project exists
        var query = $"SELECT * FROM Project WHERE project_id = {project.Project_id};";
        var command = new SqliteCommand(query, connection);
        var reader = command.ExecuteReader();

        if (!reader.Read())
        {
            return false; // Project does not exist, return false
        }

        // Updating the project
        query = $"UPDATE Project SET name = @Name, description = @Description, project_manager = @ProjectManager WHERE project_id = @Project_id;";
        command = new SqliteCommand(query, connection);
        command.Parameters.AddWithValue("@Name", project.Name);
        command.Parameters.AddWithValue("@Description", project.Description);
        command.Parameters.AddWithValue("@ProjectManager", project.ProjectManager);
        command.Parameters.AddWithValue("@Project_id", project.Project_id);
        command.ExecuteNonQuery();

        connection.Close();

        return true;
    }


    public bool DeleteProject(int id)
    {
        connection.Open();

        // Checking if the project exists
        var query = $"SELECT * FROM Project WHERE project_id = {id};";
        var command = new SqliteCommand(query, connection);
        var reader = command.ExecuteReader();

        if (!reader.Read())
        {
            return false; // Project does not exist, return false
        }

        // Checking if there are epics associated with the project
        query = $"SELECT * FROM Epic WHERE project_id = {id};";
        command = new SqliteCommand(query, connection);
        reader = command.ExecuteReader();

        if (reader.Read())
        {
            return false; // There are epics associated with the project, return false
        }

        // Deleting the project
        query = $"DELETE FROM Project WHERE project_id = {id};";
        command = new SqliteCommand(query, connection);
        command.ExecuteNonQuery();

        connection.Close();

        return true;
    }

    public List<EpicItem> GetEpics()
    {
        connection.Open();

        var query = "SELECT * FROM Epic;";
        var command = new SqliteCommand(query, connection);
        var reader = command.ExecuteReader();

        var epics = new List<EpicItem>();
        while (reader.Read())
        {
            var epic = ReadEpicItem(reader);
            epics.Add(epic);
        }

        connection.Close();

        return epics;
    }

    public EpicItem? GetEpicById(int id)
    {
        connection.Open();

        // Avoiding SQL injection
        var query = $"SELECT * FROM Epic WHERE epic_id = @id;";
        var command = new SqliteCommand(query, connection);
        command.Parameters.AddWithValue("@id", id);
        var reader = command.ExecuteReader();

        EpicItem? epic = null;
        if (reader.Read())
        {
            epic = ReadEpicItem(reader);
        }

        connection.Close();

        return epic;
    }

    public EpicItem ReadEpicItem(SqliteDataReader reader)
    {
        var epic = new EpicItem
        {
            Epic_id = reader.GetInt32(0),
            Name = reader.GetString(1),
            Description = reader.GetString(2),
            Project_id = reader.GetInt32(3)
        };
        return epic;
    }

    public bool AddEpic(EpicItem epic)
    {
        connection.Open();

        var query = "SELECT * FROM Epic WHERE name = @name;";
        var command = new SqliteCommand(query, connection);
        command.Parameters.AddWithValue("@name", epic.Name);
        var reader = command.ExecuteReader();

        if (reader.Read())
        {
            return false; // Epic already exists, return false
        }

        var insertQuery = "INSERT INTO Epic (name, description, project_id) VALUES (@Name, @Description, @Project_id);";
        var insertCommand = new SqliteCommand(insertQuery, connection);
        insertCommand.Parameters.AddWithValue("@Name", epic.Name);
        insertCommand.Parameters.AddWithValue("@Description", epic.Description);
        insertCommand.Parameters.AddWithValue("@Project_id", epic.Project_id);
        insertCommand.ExecuteNonQuery();

        connection.Close();

        return true;
    }

    public bool UpdateEpic(EpicItem epic)
    {
        connection.Open();

        // Checking if the epic exists
        var query = $"SELECT * FROM Epic WHERE epic_id = {epic.Epic_id};";
        var command = new SqliteCommand(query, connection);
        var reader = command.ExecuteReader();

        if (!reader.Read())
        {
            return false; // Epic does not exist, return false
        }

        // Updating the epic
        query = $"UPDATE Epic SET name = @Name, description = @Description, project_id = @Project_id WHERE epic_id = @Epic_id;";
        command = new SqliteCommand(query, connection);
        command.Parameters.AddWithValue("@Name", epic.Name);
        command.Parameters.AddWithValue("@Description", epic.Description);
        command.Parameters.AddWithValue("@Project_id", epic.Project_id);
        command.Parameters.AddWithValue("@Epic_id", epic.Epic_id);
        command.ExecuteNonQuery();

        connection.Close();

        return true;
    }

    public bool DeleteEpic(int id)
    {
        connection.Open();

        // Checking if the epic exists
        var query = $"SELECT * FROM Epic WHERE epic_id = {id};";
        var command = new SqliteCommand(query, connection);
        var reader = command.ExecuteReader();

        if (!reader.Read())
        {
            return false; // Epic does not exist, return false
        }
        // Not allowing delete if there av subtasks associated with the epic
        query = $"SELECT * FROM SubTask WHERE epic_id = {id};";
        command = new SqliteCommand(query, connection);
        reader = command.ExecuteReader();

        if (reader.Read())
        {
            return false; // There are subtasks associated with the epic, return false
        }
        
        // Deleting the epic
        query = $"DELETE FROM Epic WHERE epic_id = {id};";
        command = new SqliteCommand(query, connection);
        command.ExecuteNonQuery();

        connection.Close();

        return true;
    }


    public List<SubTaskItem> GetSubTasks()
    {
        connection.Open();

        var query = "SELECT * FROM SubTask;";
        var command = new SqliteCommand(query, connection);
        var reader = command.ExecuteReader();

        var subtasks = new List<SubTaskItem>();
        while (reader.Read())
        {
            var subtask = ReadSubTaskItem(reader);
            subtasks.Add(subtask);
        }

        connection.Close();

        return subtasks;
    }

    public SubTaskItem? GetSubTaskById(int id)
    {
        connection.Open();

        // Avoiding SQL injection
        var query = $"SELECT * FROM SubTask WHERE subtask_id = @id;";
        var command = new SqliteCommand(query, connection);
        command.Parameters.AddWithValue("@id", id);
        var reader = command.ExecuteReader();

        SubTaskItem? subtask = null;
        if (reader.Read())
        {
            subtask = ReadSubTaskItem(reader);
        }

        connection.Close();

        return subtask;
    }

    public SubTaskItem ReadSubTaskItem(SqliteDataReader reader)
    {
        var subtask = new SubTaskItem
        {
            Subtask_id = reader.GetInt32(0),
            TaskName = reader.GetString(1),
            Description = reader.GetString(2),
            Epic_id = reader.GetInt32(3)
        };
        return subtask;
    }

    public bool AddSubTask(SubTaskItem subtask)
    {
        connection.Open();

        var query = "SELECT * FROM SubTask WHERE subtask_id = @id;";
        var command = new SqliteCommand(query, connection);
        command.Parameters.AddWithValue("@id", subtask.Subtask_id);
        var reader = command.ExecuteReader();

        if (reader.Read())
        {
            return false; // SubTask already exists, return false
        }

        var insertQuery = "INSERT INTO SubTask (name, description, epic_id) VALUES (@Name, @Description, @Epic_id);";
        var insertCommand = new SqliteCommand(insertQuery, connection);
        insertCommand.Parameters.AddWithValue("@Name", subtask.TaskName);
        insertCommand.Parameters.AddWithValue("@Description", subtask.Description);
        insertCommand.Parameters.AddWithValue("@Epic_id", subtask.Epic_id);
        insertCommand.ExecuteNonQuery();

        connection.Close();

        return true;

    }

    public bool UpdateSubTask(SubTaskItem subtask)
    {
        connection.Open();

        // Checking if the subtask exists
        var query = $"SELECT * FROM SubTask WHERE subtask_id = {subtask.Subtask_id};";
        var command = new SqliteCommand(query, connection);
        var reader = command.ExecuteReader();

        if (!reader.Read())
        {
            return false; // SubTask does not exist, return false
        }

        // Updating the subtask
        query = $"UPDATE SubTask SET name = @Name, description = @Description, epic_id = @Epic_id WHERE subtask_id = @Subtask_id;";
        command = new SqliteCommand(query, connection);
        command.Parameters.AddWithValue("@Name", subtask.TaskName);
        command.Parameters.AddWithValue("@Description", subtask.Description);
        command.Parameters.AddWithValue("@Epic_id", subtask.Epic_id);
        command.Parameters.AddWithValue("@Subtask_id", subtask.Subtask_id);
        command.ExecuteNonQuery();

        connection.Close();

        return true;
    }

    public bool DeleteSubTask(int id)
    {
        connection.Open();

        // Checking if the subtask exists
        var query = $"SELECT * FROM SubTask WHERE subtask_id = {id};";
        var command = new SqliteCommand(query, connection);
        var reader = command.ExecuteReader();

        if (!reader.Read())
        {
            return false; // SubTask does not exist, return false
        }

        // Deleting the subtask
        query = $"DELETE FROM SubTask WHERE subtask_id = {id};";
        command = new SqliteCommand(query, connection);
        command.ExecuteNonQuery();

        connection.Close();

        return true;
    }





}
