﻿namespace VaccinationCardManagement.Application.Models;

public record UserModel
{
    public int Id { get; init; }
    public string Name { get; init; }
    public string Email { get; init; }
}