using System;
using System.Collections.Generic;
using System.Linq;
using FeedIt.Data.Models;

namespace FeedIt.Data
{
    public class UserQueryFilters
    {
        public enum DegreesOfPopularity
        {
            Any = 0,
            Unknown,
            Important,
            Popular
        }

        public enum ArticleCounts
        {
            Any = 0,
            Few,
            Medium,
            Many
        }

        public static Dictionary<DegreesOfPopularity, string> PopularityLabels =
            new Dictionary<DegreesOfPopularity, string>
            {
                { DegreesOfPopularity.Any, "Любой" },
                { DegreesOfPopularity.Popular, "Популярный" },
                { DegreesOfPopularity.Important, "Важный" },
                { DegreesOfPopularity.Unknown, "Неизвестный" }
            };

        public static Dictionary<ArticleCounts, string> ArticleCountsLabels =
            new Dictionary<ArticleCounts, string>
            {
                { ArticleCounts.Any, "Любое" },
                { ArticleCounts.Many, "Много" },
                { ArticleCounts.Medium, "Средне" },
                { ArticleCounts.Few, "Мало" }
            };


        public string NameToSearch { get; set; }

        public DegreesOfPopularity Popularity
        {
            get => _popularity;
            set
            {
                if (value < 0 || value > (DegreesOfPopularity) 3)
                {
                    _popularity = DegreesOfPopularity.Any;
                }
                else
                    _popularity = value;
            }
        }

        public ArticleCounts ArticleCount
        {
            get => _articleCount;
            set
            {
                if (value < 0 || value > (ArticleCounts) 3)
                {
                    _articleCount = ArticleCounts.Any;
                }
                else
                    _articleCount = value;
            }
        }

        private DegreesOfPopularity _popularity;
        private ArticleCounts _articleCount;

        public UserQueryFilters()
        {
            Popularity = DegreesOfPopularity.Any;
            ArticleCount = ArticleCounts.Any;
            NameToSearch = "";
        }

        public IQueryable<User> BuildQuery(IQueryable<User> query)
        {
            switch (Popularity)
            {
                case DegreesOfPopularity.Unknown:
                    query = query
                        .Where(user => user.Subscribers
                                           .Count() <= 10);
                    break;
                case DegreesOfPopularity.Important:
                    query = query
                        .Where(user => user.Subscribers
                                           .Count() > 10 && user.Subscribers
                                           .Count() < 100);
                    break;
                case DegreesOfPopularity.Popular:
                    query = query
                        .Where(user => user.Subscribers
                                           .Count() >= 100);
                    break;
                default:
                    //ignore
                    break;
            }

            switch (ArticleCount)
            {
                case ArticleCounts.Few:
                    query = query
                        .Where(user => user.Articles
                                           .Count(article => article.IsPublic) <= 5);
                    break;
                case ArticleCounts.Medium:
                    query = query
                        .Where(user =>
                            user.Articles
                                .Count(article => article.IsPublic) > 5 &&
                            user.Articles
                                .Count(article => article.IsPublic) < 25);
                    break;
                case ArticleCounts.Many:
                    query = query
                        .Where(user => user.Articles
                                           .Count(article => article.IsPublic) >= 25);
                    break;
                default:
                    //ignore
                    break;
            }

            if (!string.IsNullOrWhiteSpace(NameToSearch))
            {
                query = query
                    .Where(user => user.PublicName.Contains(NameToSearch.Trim()));
            }

            return query;
        }
    }
}