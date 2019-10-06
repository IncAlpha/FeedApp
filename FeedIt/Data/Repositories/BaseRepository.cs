using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FeedIt.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace FeedIt.Data.Repositories
{
    public abstract class BaseRepository<TModel>
        where TModel : BaseModel
    {
        protected readonly AppDbContext Context;

        public abstract DbSet<TModel> DbSet { get; }

        /// <summary>
        /// Returns all models;
        /// </summary>
        public IEnumerable<TModel> Entries => DbSet.AsEnumerable();

        protected BaseRepository(AppDbContext context)
        {
            Context = context;
        }

        /// <summary>
        /// Is model exist in database;
        /// </summary>
        /// <param name="model">Target model object</param>
        public Task<bool> IsExist(TModel model)
        {
            return DbSet.AnyAsync(entry => entry.Id == model.Id);
        }

        /// <summary>
        /// Is model exist in database;
        /// </summary>
        /// <param name="id">Target model id</param>
        public Task<bool> IsExist(Guid id)
        {
            return DbSet.AnyAsync(entry => entry.Id == id);
        }

        /// <summary>
        /// Get model object by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Found model or null</returns>
        public Task<TModel> GetById(Guid id)
        {
            return DbSet.FirstOrDefaultAsync(model => model.Id == id);
        }

        public async Task Save(TModel model)
        {
            var exist = await IsExist(model);
            Context.Entry(model).State = !exist ? EntityState.Added : EntityState.Modified;

            await Context.SaveChangesAsync();
        }

        public async Task Delete(TModel model)
        {
            DbSet.Remove(model);
            await Context.SaveChangesAsync();
        }
    }
}