using System.Collections;
using System.Collections.Generic;

public class AIStateMachine
{

	int playCooldown = 2;

	float lastPlayCooldownTimer = 0;
	public void Init()
	{

	}

	public void Tick()
	{
		//UnityEngine.Debug.Log("Ticking AI");

		List<Enemy> enemies = Main.instance.GetCombatStateMachine().GetActiveEnemies();
		if(enemies.Count <=0)
		{
			EndAITurn();
			UnityEngine.Debug.Log("AI ending Turn, all enemies dead... TODO: Should this happen?");
			return;
		}
		
		if(lastPlayCooldownTimer <= 0)
		{
			foreach(Enemy e in enemies)
			{
				//UnityEngine.Debug.Log("Enemy: " + e.GetName() + " has " + e.GetCurrentHand().DeckCount() + " cards in hand");
				for(int i = 0; i < e.GetCurrentHand().DeckCount(); i++)
				{
					Card c = e.GetCurrentHand().GetContents()[i];
					if(CanPlayCard(c) == true)
					{
						Main.instance.EnemyPlayCard(c, e);
						lastPlayCooldownTimer = playCooldown;
						UnityEngine.Debug.Log("AI playing card " + c.GetCardType());
						return;
					
					}
				}
				
			}
			UnityEngine.Debug.Log("AI ending turn, no valid actions");
			EndAITurn();
		}
		else
		{
			//UnityEngine.Debug.Log("AI Cooling Down");
			lastPlayCooldownTimer -= UnityEngine.Time.deltaTime;
		}



	}

	bool CanPlayCard(Card c)
	{
		return true;
	}

	void EndAITurn()
	{
		Main.instance.AIEndTurn();
	}
}
