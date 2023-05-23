using System;
using System.Collections.Generic;
using System.Text;

namespace survSimProject
{
    class Island
    {
        public int id;
        public List<Sector> sectorList = new List<Sector>();
        public int campSectorID;
        public SectorCamp camp;

        public Island(int _id)
        {
            id = _id;
            int campPosX = Misc.rng.Next(1, GlobSettings.islandSize - 1);
            int campPosY = Misc.rng.Next(1, GlobSettings.islandSize - 1);
            int counter = 0;
            for (int i = 0; i < GlobSettings.islandSize; i++)
            {
                for (int j = 0; j < GlobSettings.islandSize; j++)
                {
                    if (i == 0 || i == GlobSettings.islandSize - 1 || j == 0 || j == GlobSettings.islandSize - 1) sectorList.Add(new SectorShore(counter, j, i));
                    else if (i == campPosX && j == campPosY)
                    {
                        sectorList.Add(new SectorCamp(counter, j, i)); 
                        campSectorID = counter;
                    }
                    else sectorList.Add(new Sector(counter, j, i));
                    counter++;
                }
            }
            camp = (SectorCamp)sectorList[campSectorID];
        }
    }
}
