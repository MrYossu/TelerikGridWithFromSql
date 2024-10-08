# Using a Telerik Blazor grid with the EF Core `FromSqlRaw` method

Following my [experiments in using Dapper](https://github.com/MrYossu/TelerikGridWithDapper) to improve the performance of the Telerik Blazor grid, a colleague reminded me that EF Core has a `FromSqlRaw` method, which basically does the same as what I was doing in Dapper.

This repo is the playground for my attempt (and eventual success) in writing a version of my Dapper method that uses EF Core.

Along the way, I fixed a bug or two that we noticed with the Dapper version of the code, and I also added some significant improvements to the functionality.

A blog post will hopefully follow with more details. If you want to use it in the meantime, see the code in this repo.