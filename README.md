# Using a Telerik Blazor grid with the EF Core `FromSqlRaw` method

Following my [experiments in using Dapper](https://github.com/MrYossu/TelerikGridWithDapper) to improve the performance of the Telerik Blazor grid, a colleague reminded me that EF Core has a `FromSqlRaw` method, which basically does the same as what I was doing in Dapper.

This repo is the playground for my attempt (and eventual success) in writing a version of my Dapper method that uses EF Core.

Along the way, I fixed a bug or two that we noticed with the Dapper version of the code, and I also added some significant improvements to the functionality.

Three blog posts may or may not be relevant. OK, so the third is, the other two may not 😎.

## Bed time reading
If you're bored, you can read [this blog post](https://www.pixata.co.uk/2024/09/09/using-dapper-with-the-telerik-blazor-grid/), which explains my experiment with using Dapper to increase performance. The Dapper code is in the repo linked above.

## Bit long, but background to this repo
However, after my colleague pointed out that EF Core has a method that allows you to do the same thing, I wondered why not use that rather than Dapper? Good question, and one I explore (along with quite a bit of rambling and a couple of anecdotes that show how old I am!) in [this blog post](https://www.pixata.co.uk/2024/10/08/hmm-maybe-ef-core-isnt-so-bad-after-all/).

## The one you actually need to read
The end result was that I rewrote the Dapper code to use EF Core, and published in one of my Nuget packages. [This blog post](https://www.pixata.co.uk/2024/10/09/using-the-ef-core-fromsqlraw-method-in-a-telerik-blazor-grid/) explains how to use it.