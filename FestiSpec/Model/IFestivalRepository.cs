using FestiSpec.Entity;
using System.Collections.Generic;

namespace FestiSpec.Model
{
    interface IFestivalRepository
    {
        List<Festival> GetFestivals();
        List<Festival> GetFestivals(int clientId);
    }
}