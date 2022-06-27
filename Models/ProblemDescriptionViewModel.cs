﻿namespace ToDoList21.Models
{
    public class ProblemDescriptionViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public string Description { get; set; }

        public ProblemDescriptionViewModel(Problem model)
        {
            Id = model.Id;
            Title = model.Title;
            Description = model.Description;
        }
    }
}