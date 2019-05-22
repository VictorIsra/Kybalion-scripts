using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*  ESSA CLASSE GERENCIA O STATUS DO PLAYER:
    SUA VIDA, CHANCES, SCORE, ARMAS  E COMPORTAMENTO. É COMPLEMENTAR AO <Player.cs> 
*/

public class PlayerStatus : MonoBehaviour{
   	
    private Player player;
    private Level level;
    private UiManagement uiManagement;
    private bool freezePlayerControl;
    
    
    /*MÉTODOS DO PLAYER QUE SE RELACIONAM COM O EVENTO DE GAMEOVER: */
    public void gameOverTrigger(){
		try{
            /* resetarei o status do player invocando PlayerGlobalStatus.resetPlayerStatus()
            na própria classe level, para garantir que o método só será chamado
            após a cena do game over*/
            setLevelRerence();
			level.triggerGameOverScene();
		}catch(System.Exception e){
			Debug.LogWarning(e);
		}
	}
    public bool isGameOver(){
        return PlayerGlobalStatus.getPlayerChances() <= 0;
    }
    /*MÉTODOS QUE CRIAM REFERENCIAS E SETAM STATUS: */
    private void setLevelRerence(){
        try{
            level = FindObjectOfType<Level>();
            if(!level)
                return;
        }catch(System.Exception e){
            Debug.LogWarning(e);
        }    
    }
    /*MÉTODOS REFERENTES A PERMITIR OU NÃO QUE POSSA MOVER O PERSONAGEM */ 
    public bool getFreezePlayerControlStatus(){
		return freezePlayerControl;
	}
    public void setFreezePlayerControlStatus(bool status){
		freezePlayerControl = status;
	}
}