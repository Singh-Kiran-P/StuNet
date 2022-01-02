using System;
using FuzzySharp;
using Server.Api.Dtos;
using Server.Api.Models;
using System.Collections.Generic;

namespace Server.Api.Services
{
    public class StringMatcher
    {
        private static int maxItems = 100;
        private static int matchPercent = 60;

        private static Boolean Match(string target, string search)
        {
            if (search == null || search == "") return true;
            var match = Fuzz.PartialRatio(target.ToLower(), search.ToLower());
            return match >= matchPercent;
        }

        public static IEnumerable<T> FuzzyMatchObject<T>(IEnumerable<T> obj, string search)
        {
            Type t = typeof(T);
            List<T> matches = new List<T>();
            foreach (T item in obj) {
                if (matches.Count >= maxItems) break;
                switch (item) {
                    case GetAllCourseDto course:
                        if (Match(course.name, search)) matches.Add(item);
                        else if (Match(course.number, search)) matches.Add(item);
                        else if (Match(course.description, search)) matches.Add(item);
                        break;
                    case Question question:
                        if (Match(question.title, search)) matches.Add(item);
                        else if (Match(question.body, search)) matches.Add(item);
                        break;
                    default: break;
                }
            }
            return matches;
        }
    }
}
