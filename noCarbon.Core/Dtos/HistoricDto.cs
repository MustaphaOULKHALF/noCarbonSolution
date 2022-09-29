﻿namespace noCarbon.Core.Dtos;

public class HistoricDto
{
    public HistoricDto()
    {

    }
    public Guid CustomerId { get; set; }
    public string CategoryName { get; set; }
    public string ActionName { get; set; }
    public int Points { get; set; }
    public decimal ReducedCarb { get; set; }
    public DateTime OperationDate { get; set; }
    public TimeSpan OperationTime { get; set; }
}
