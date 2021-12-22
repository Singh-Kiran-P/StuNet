// @Kiran

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FuzzySharp;
using Server.Api.Dtos;
using Server.Api.Models;

namespace Server.Api.Services
{
    public class StringMatcher
    {
        private static int StringMatchPercent = 45;
        public static IEnumerable<T> FuzzyMatchObject<T>(IEnumerable<T> obj, string searchString)
        {
            Type t = typeof(T);
            List<T> outList = new List<T>();

            foreach (T item in obj)
            {
                switch (item)
                {
                    case GetAllCourseDto dto:
                        int match_ = 0;
                        if (searchString == null)
                            match_ = 100;
                        else
                            match_ = Fuzz.PartialRatio(dto.name, searchString);
                        // match_ = match_ - (100 - Fuzz.Ratio (dto.name, searchString));

                        Console.WriteLine($"{match_} - {searchString} - {dto.name}");
                        if (match_ >= StringMatchPercent)
                            outList.Add(item);

                        break;
                    default:
                        // Handle this case.
                        Console.WriteLine("I don't know what this type is.");
                        break;
                }
            }
            return outList;
        }
    }
}
