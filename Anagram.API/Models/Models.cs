using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;


namespace AnagramAPI
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Microsoft.EntityFrameworkCore.DbContext" />
    public class DictionaryDBContext : DbContext
    {
        /// <summary>
        /// Gets or sets the dictionary.
        /// </summary>
        /// <value>
        /// The dictionary.
        /// </value>
        public DbSet<DictionaryDB> Dictionary { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="DictionaryDBContext"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public DictionaryDBContext(DbContextOptions<DictionaryDBContext> options): base(options)
        { 
        }

        /// <summary>
        /// Override this method to further configure the model that was discovered by convention from the entity types
        /// exposed in <see cref="T:Microsoft.EntityFrameworkCore.DbSet`1" /> properties on your derived context. The resulting model may be cached
        /// and re-used for subsequent instances of your derived context.
        /// </summary>
        /// <param name="modelBuilder">The builder being used to construct the model for this context. Databases (and other extensions) typically
        /// define extension methods on this object that allow you to configure aspects of the model that are specific
        /// to a given database.</param>
        /// <remarks>
        /// If a model is explicitly set on the options for this context (via <see cref="M:Microsoft.EntityFrameworkCore.DbContextOptionsBuilder.UseModel(Microsoft.EntityFrameworkCore.Metadata.IModel)" />)
        /// then this method will not be run.
        /// </remarks>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DictionaryDB>().HasKey(k => k.Word_ID);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class DictionaryDB
    {
        /// <summary>
        /// Gets the word identifier.
        /// </summary>
        /// <value>
        /// The word identifier.
        /// </value>
        [Key]
        public int? Word_ID { get; }
        /// <summary>
        /// Gets or sets the word.
        /// </summary>
        /// <value>
        /// The word.
        /// </value>
        public string Word { get; set; }
        /// <summary>
        /// Gets or sets the word ordered array.
        /// </summary>
        /// <value>
        /// The word ordered array.
        /// </value>
        public string Word_ordered_array { get; set; }
        /// <summary>
        /// Gets or sets the type of the word.
        /// </summary>
        /// <value>
        /// The type of the word.
        /// </value>
        public string Word_type { get; set; }
    }
}
