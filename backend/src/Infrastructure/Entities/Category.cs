﻿namespace Infrastructure.Entities;

public sealed class Category : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public ICollection<Product> Products { get; set; } = new List<Product>();
}