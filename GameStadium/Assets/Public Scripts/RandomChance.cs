using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
	 public static class RandomChance
	 {
		  public static bool GetRandomChance(int chanceInPercent)
		  {
				int number = UnityEngine.Random.Range(0, 100);

				if (number <= chanceInPercent)
				{
					 return true;
				}
				else
				{
					 return false;
				}
		  }
	 }
}
