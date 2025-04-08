using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RazorMovies.Data;
using RazorMovies.Models;

namespace RazorMovies.Pages.Movies
{
    public class IndexModel : PageModel
    {
        private readonly RazorMovies.Data.RazorMoviesContext _context;

        public IndexModel(RazorMovies.Data.RazorMoviesContext context)
        {
            _context = context;
        }

        public IList<Movie> Movie { get; set; } = default!;

        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? MovieGenre { get; set; }
        public SelectList? Genres { get; set; }

        public async Task OnGetAsync()
        {
            var movies = from m in _context.Movie
                         select m;
            if (!string.IsNullOrEmpty(SearchString))
            {
                movies = movies.Where(s => s.Title!.Contains(SearchString));
            }

            if(!string.IsNullOrEmpty(MovieGenre))
            {
                movies = movies.Where(x => x.Genre == MovieGenre);
            }
            Genres = new SelectList(await _context.Movie.OrderBy(x => x.Genre).Select(x => x.Genre).Distinct().ToListAsync());

            Movie = await movies.ToListAsync();
        }
    }
}
