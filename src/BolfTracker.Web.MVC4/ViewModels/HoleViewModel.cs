using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BolfTracker.Models;

namespace BolfTracker.Web.ViewModels
{
    //Id Name	            Description
    //1	 Make	            Player made the shot
    //2	 Miss	            Player missed the shot
    //3	 Push	            A player pushes the points to the next hole
    //4	 Steal	            A player steals the points from another player
    //5	 Sugar-Free Steal	A player steals the points when the hole is already pushed
    public class HoleResult
    {
        public HoleResult()
        {
            IsEmpty = true;
        }

        public Shot Shot          { get; set; }
        public Player LastPlayer  { get; set; }
        public Player FirstPlayer { get; set; }

        public bool IsEmpty { get; set; }

        public bool IsPush
        {
            get
            {
                return Shot.ShotType.Id == 3;
            }
        }
        public bool IsSteal
        {
            get
            {
                return Shot.ShotType.Id == 4;
            }
        }
        public bool IsSugarFreeSteal
        {
            get
            {
                return Shot.ShotType.Id == 5;
            }
        }
        
        const string MADE_FORMAT = "{0} Wins Hole";
        const string PUSH_FORMAT  = "{0} Pushed {1}";
        const string STEAL_FORMAT = "{0} Stole From {1}";
        const string SFS_FORMAT   = "{0} SFS From {1}";

        public string ShotDisplayName
        {
            get
            {
                //No shots made
                if (IsEmpty)
                {
                    return "";
                }
                else
                {
                    if (Shot == null)
                    {
                        return "Infinity";
                    }
                    else
                    {
                        switch (Shot.ShotType.Id)
                        {
                            case 1:
                                return string.Format(MADE_FORMAT, LastPlayer.Initials);
                            case 3:
                                return string.Format(PUSH_FORMAT, LastPlayer.Initials, FirstPlayer.Initials);
                            case 4:
                                return string.Format(STEAL_FORMAT, LastPlayer.Initials, FirstPlayer.Initials);
                            case 5:
                                return string.Format(SFS_FORMAT, LastPlayer.Initials, FirstPlayer.Initials);
                            default:
                                return "Infinity";
                        }
                    }
                }
            }
        }

        public int Points { get; set; }
    }

    public class HoleViewModel
    {
        public HoleViewModel(Game game)
        {
            var holes =  (from s in game.Shots
                         select s.Hole).Distinct();

            Holes = holes;
        }

        public HoleViewModel(IEnumerable<Hole> holes)
        {
            Holes = holes;
        }

        public IEnumerable<Hole> Holes { get; set; }
        
        public HoleResult GetHoleResult(Hole hole)
        {
            HoleResult result = new HoleResult();

            if (hole.Shots != null && hole.Shots.Any())
            {
                result.IsEmpty = false;
                //Get the last shot
                var shot = hole.Shots.FirstOrDefault(s => s.ShotMade == true || s.ShotType.Id == 3);
                
                //If it was made, see if it was a push, steal
                if (shot != null && shot.ShotMade == true)
                {
                    result.Shot = shot;
                    result.LastPlayer = shot.Player;

                    //If a push
                    switch (shot.ShotType.Id)
                    {
                        case 1 :
                            result.FirstPlayer = null;
                            result.Points = hole.Par;
                            break;
                        case 3 :
                            var pushedShot = hole.Shots.FirstOrDefault(s => s.Attempts == shot.Attempts && s.Id != shot.Id && s.ShotMade == true);
                            result.FirstPlayer = pushedShot.Player;
                            break;                            
                        case 4 :
                            var stolenShot = hole.Shots.LastOrDefault(s => s.Attempts > shot.Attempts && s.ShotMade == true);
                            result.FirstPlayer = stolenShot.Player;
                            result.Points = hole.Par;
                            break;
                        //Try and find the players who pushed the hole
                        case 5 :
                            var lastPushedShot  = hole.Shots.LastOrDefault(s => s.ShotMade == true && s.Attempts > shot.Attempts);
                            var firstPushedShot = hole.Shots.LastOrDefault(s => s.ShotMade == true && s.Attempts == lastPushedShot.Attempts && s.Id != lastPushedShot.Id);
                            result.FirstPlayer = new Player() { Initials = lastPushedShot.Player.Initials + "/" + firstPushedShot.Player.Initials };
                            result.Points = hole.Par;

                            break;
                        default :
                            result.FirstPlayer = null;
                            break;
                    }

                    if (result.Points > 0)
                    {
                        //loop through previous holes adding up score until last winning shot.
                        for (int i = hole.Id - 1; i > 0; i--)
                        {
                            var previousHole = Holes.FirstOrDefault(h => h.Id == i);
                            //Check for push
                            if ((previousHole.Shots.Count(s => s.ShotMade == true && s.ShotType.Id == 3) > 0) || previousHole.Shots.Count(s => s.ShotMade == true) == 0)
                            {
                                result.Points += previousHole.Par;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
            }

            return result;
        }
    }
}