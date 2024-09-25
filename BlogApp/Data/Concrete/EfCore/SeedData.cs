using BlogApp.Entity;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Data.Concrete.EfCore
{

    public static class SeedData //static ile disaridan kullanilabilir hale getirdim.
    {

        public static void TestVerileriniDoldur(IApplicationBuilder app)
        {

            var context = app.ApplicationServices.CreateScope().ServiceProvider.GetService<BlogContext>();

            if (context != null)
            {
                if (context.Database.GetPendingMigrations().Any())
                {
                    context.Database.Migrate();
                }

                if (!context.Tags.Any())
                {
                    context.Tags.AddRange(
                        new Tag { Text = "egypt", Url = "egypt", Color = TagColors.primary },
                        new Tag { Text = "jordan", Url = "jordan", Color = TagColors.danger },
                        new Tag { Text = "petra", Url = "petra", Color = TagColors.info },
                        new Tag { Text = "pyramids", Url = "pyramids", Color = TagColors.success },
                        new Tag { Text = "bosnia", Url = "bosnia", Color = TagColors.secondary }
                    );
                    context.SaveChanges();
                }
                if (!context.Users.Any())
                {
                    context.Users.AddRange(
                        new User { UserName = "sedefnurkayran", Name = "Sedefnur Kayran", Email = "info@sedefnurkayran.com", Password = "123456,", Image = "sedefnurkayran.PNG" },
                        new User { UserName = "sefademir", Name = "Sefa Demir", Email = "info@sefademir.com", Password = "123456,", Image = "p2.jpg" }
                    );
                    context.SaveChanges();
                }
                if (!context.Posts.Any())
                {
                    context.Posts.AddRange(
                        new Post
                        {
                            Title = "The lost city carved into the mountains: Petra Ancient City",
                            Content = "Petra Ancient City, a unique destination where history, nature and architecture come together",
                            Description = "A cultural heritage on the UNESCO World Heritage List",
                            Url = "petra-jordan",
                            IsActive = true,
                            PublishedOn = DateTime.Now.AddDays(-15),
                            // Tags = context.Tags.Take(1).ToList(),
                            Tags = context.Tags.Where(t => t.Text == "jordan" || t.Text == "petra").ToList(),
                            Image = "petra1.jpg",
                            UserId = 1,
                            Comments = new List<Comment>{
                                new Comment {Text = "I recommend you to visit Petra", PublishedOn = new DateTime(),UserId=1},
                                new Comment {Text = "One of the new seven wonders of the world ", PublishedOn = new DateTime(),UserId=2}
                            }
                        },
                        new Post
                        {
                            Title = "Egypt Pyramids",
                            Content = "The most famous of these magnificent structures, large and small, numbering more than a hundred, is the Pyramid of Cheops.",
                            Description = "The Mystery of the Pyramids",
                            Url = "One-of-7-wonders-of-the-world-the-pyramids-of-gize",
                            IsActive = true,
                            PublishedOn = DateTime.Now.AddDays(-10),
                            // Tags = context.Tags.Take(2).ToList(),
                            Tags = context.Tags.Where(t => t.Text == "egypt" || t.Text == "pyramids").ToList(),
                            Image = "misir.jpg",
                            UserId = 2
                        },
                        new Post
                        {
                            Title = "Vrelo Bosne",
                            Content = "An incredibly beautiful natural park consisting of streams and ponds where you can find every shade of green together.",
                            Description = "The Green Heart of Sarajevo",
                            Url = "Bosna-Spring",
                            IsActive = true,
                            PublishedOn = DateTime.Now.AddDays(-5),
                            // Tags = context.Tags.Take(4).ToList(),
                            Tags = context.Tags.Where(t => t.Text == "bosnia").ToList(),
                            Image = "bosna.jpg",
                            UserId = 1
                        }
                    );
                    context.SaveChanges();
                }
            }
        }
    }
}