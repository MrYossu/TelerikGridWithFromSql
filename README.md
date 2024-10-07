# Using a Telerik Blazor grid with the EF Core `FromSqlRaw` method

Following my experiments in using Dapper to improve the performance of the Telerik Blazor grid, a colleague reminded me that EF Core has a `FromSqlRaw` extension method, which basically does the same as what I was doing in Dapper.

This repo is the playground for my attempt (and eventual success) in writing a version of my Dapper method that uses EF Core.
