using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constantes{
	//CENAS:
	public static string LEVEL1 = "Level1";
	public static string LEVEL2 = "Level2";
	public static string START_MENU ="Menu";
	public static string TRY_AGAIN = "TryAgain";
	public static string GAME_OVER ="GameOver";
	public static string SHOP_MENU = "Shop";
	public static string HOW_TO_MENU = "HowTo";
	public static string HOW_TO_MENU2 = "HowTo2";
	public static string CREDITS_MENU = "Credits";
	public static string NONE = "NONE";/*pro fadescreenmanager saber que nao tem que carregar nada sozinho xD */
	public static string QUIT = "QUIT";
	public static string SAME_LEVEL = "SameLevel";
	public static string NEXT_LEVEL = "NextLevel";
	public static string SHOP_LEVEL = "Shop";
	//OBJETOS/CLASSES/TAGS NO GERAL:
	public static string PLAYER = "Player";
	public static string PLAYER_SMOKE_SPAW_POINT = "PlayerSmokeSpawPoint";
	public static string PLAYER_UPGRADE_SPAWPOINT_A = "PlayerUpgradeSpawPointA";
	public static string PLAYER_UPGRADE_SPAWPOINT_B = "PlayerUpgradeSpawPointB";
	public static string ENEMYPROJETIL = "Projectil";
	public static string PLAYERPROJETIL= "PlayerProjectil";
	public static string VIDACOLETABLE = "VidaColetavel";
	public static string MUNICAOCOLETABLE = "MunicaoColetavel";
	public static string PONTUACAOCOLETABLE = "PontuacaoColetavel";
	public static string ENEMY = "Enemy";
	public static string BOSS= "Boss";
	public static string obstaculoInimigo = "ObstaculoInimigo";
	public static string CASH_ = "Cash";
	public static string SCENE_OBJECT = "sceneObject";
	public static string ROBOT_SEEK_POINT_A = "RobotSeekPointA";
	public static string ROBOT_SEEK_POINT_B = "RobotSeekPointB";
	public static string ROBOT_SEEK_POINT_C = "RobotSeekPointC";
	public static string ROBOT_SEEK_POINT_NONE =  "None";
	public static string ROBOT = "Robot";
	public static string ROBOT_TRANSFORM_CONTAINER = "RobotTransformContainer";	
	public static string RED_EYE = "RedEye";
	//public static string FIRE_AFTER_ANIMATION = "FireAfterAnimation";
	public static string SHIELD_AURA = "ShieldAura";
	public static string BOSS_EXPLOSION_SPAWNPOINT = "BossExplosionSpawnPoint";
	public static string SHADOW = "Shadow";
	//UI EM GERAL:
	public static string NAVE_MAE_LIFEBAR ="NaveMaeLifeBar";
	public static string OUT_PUT_PANEL ="outputPanel";
	public static string PLAYER_SALDO_MENU ="playerSaldoMenu";
	public static string CHANCES_UI ="ChancesUI";
	public static int NUMERO_CHANCES_DEFAULT = 3;
	public static string CASH_UI = "CashUI";
	public static string WEAPON_UI = "WeaponUI";
	public static string HEALTH_UI = "HealthUI";
	public static string PAUSE_MENU = "PauseMenu";
		/*ITENS DO PLAYER */
		public static string PLAYER_PANEL = "PlayerPanel";
		public static string ITEM_UI = "ItemUI";
		public static string ITEM_IMAGE = "Image";
		public static string ITEM_AMOUNT = "ItemAmount";


	//níveis dos itens ( 1 é dps que compra, 2 e 3 sao com upgrades)
		public static int MAX_ROBOTS_NUMBER = 3;
		public static int ITEN_BASIC_LEVEL = 1;
		public static int ITEN_INTERMEDIARY_LEVEL = 2;
		public static int ITEN_MAX_LEVEL = 3;	
		//maximo de bombas/escudos que o player pode ter por vez;
		public static int MAX_ITEM_CAPACITY = 10;
	//botoes e paines:
		public static string BUY_BUTTON = "BuyButton";
		public static string SELL_BUTTON = "SellButton";
		public static string UPGRADE_BUTTON = "UpgradeButton";
		public static string UPGRADES_PANEL = "UpgradesPanel";
		public static string ITEM_PRICE = "ItemPrice";
 		public static string ITEM_ICON_PANEL = "ItemIconPanel";	
		public static string CONFIRM_BUTTON = "ConfirmButton";
		public static string CANCEL_BUTTON = "CancelButton";
		public static string PANEL = "Panel";
		public static string CASH_PANEL ="CashPanel";
		public static string UPGRADE_INFO_PANEL = "UpgradeInfoPanel";
		/*apesar do nome, o AmmoPanels tb mostra a bomba e o shield */
		public static string AMMOS_PANELS = "AmmosPanels";//que contem:
			public static string AMMOS_IMAGE = "ammoImage";
			public static string AMMOS_INFO_PANEL= "AmmoInfoPanel";

	//icones
		public static string ITEM_ICON = "ItemIcon";
	//texts assets:
		public static string DEFAULT_OUTPUT_PANEL_TEXT = "Shop/DefaultInfoPanel/InfoPanelDefaultText";
		public static string CURRENT_AMMO_FIRE_DELAY = "CurrentAmmoFireDelay";
		public static string CURRENT_AMMO_FIRE_POWER = "CurrentAmmoFirePower";
		public static string CURREN_AMMO_FIRE_RATE = "CurrentAmmoFireRate";
		public static string BOMB_CURRENT_POWER = "BombCurrentPower";
		public static string BOMB_CURRENT_RADIUS = "BombCurrentRadius";
		public static string SHIELD_CURRENT_DURATION = "ShieldCurrentDuration";
		public static string WEAPON_INFO = "WeaponInfo";
		public static string AMMO_INFO = "AmmoInfo";
		public static string CASH = "CASH: ";
	//titulos do painel de ammo/bombas/shield:
		public static string CURRENT_AMMO_TITLE = "Current Weapons Ammos:";
		public static string BOMB_ATRIBUTES = "Bomb Atributes:";
		public static string SHIELD_ATRIBUTES = "Shield Atributes:";

	
		//item types:
		public static string WEAPON = "Weapon";
		public static string BOMB = "Bomb";
		public static string SHIELD = "Shield";
		
		//output panels ( paines que conterão textos)
		public static string INFO_PANEL_OUTPUT = "InfoPanelOutput";
		public static string AMMO_INFO_PANEL_OUTPUT = "AmmoInfoPanelOutput";
		public static string UPGRADES_INFO_PANEL_OUTPUT = "UpgradesInfoPanelOutput";


		public static string CONFIRM_BUTTON_ORIGINAL_TEXT = "OK";
		public static string CONFIRM_BUTTON_TEMP_TEXT = "No";
		
		//acao
		public static string BUY = "Buy";
		public static string SELL = "Sell";
		public static string UPGRADE = "Upgrade";

	//ANIMATION TRIGGERS:
		//FLY MISSION GATE TRIGGERS:
		public static string OPEN = "Open";
		public static string CLOSE = "Close";

		/*ROBOT TRIGGERS */
		public static string AWAKE = "Awake";
		public static string UNMORPH = "UnMorph";
	//OBJECT TRIGGERS;
		public static string OBJECT_DESTROYED = "objectDestroyed";	
		public static string ENEMY_DESTROYED = "enemyDestroyed";

	//PATHS:
	public static string DEFAULT_PLAYER_WEAPON_PATH = "Prefabs/PlayerWeapons/DefaultWeapon/w1";	//defaultAmmo/laser0";
	public static string BOMB_PATH = "Prefabs/Coletaveis/Bomb";
	public static string SHIELD_PATH = "Prefabs/Coletaveis/Shield";
	public static string GAME_AMMOS_PATH = "Prefabs/PlayerWeapons/lvl";//o codigo que usa isso ira numerar ( lvl1, lvl2 etc)
	public static string CASH_PREFAB_CASH = "Prefabs/Coletaveis/cashContainer";
	//COMPORTAMENTO DE OBJETOS DO BACKGROUND/FOREGROUND:
	public static string LOOP = "LOOP";
	public static string DESTROY = "DESTROY";

	//PORCENTAGEM DE VIDA:
	public static float AVERAGE_HEALTH = 0.55f;
	public static float LOW_HEALTH = 0.25f;
	/*eu desenho todas as municoes no sentido do player, isso aqui roda pra se adequar ao inimigo */

	public static float MIRROR_ANGLE = 180f;
	public static float VERTICAL_ANGLE = 90f;
	
	/*Não precisam mais ser utilizadas, porém são interessantes */
	public static string LEFT_SIDE = "LeftSide";
	public static string RIGHT_SIDE = "RightSide";
	public static string BOTTON_LEFT = "BottonLeft";
	public static string UPPER_LEFT = "UpperLeft";
	public static string UPPER_RIGHT = "UpperRight";
	public static string BOTTON_RIGHT = "BottonRight";
	//SORTING LAYERS
	public static string BACK_TERRAIN = "BackTerrain";
	public static string ROBOTS = "Robots";
	//ALERTAS
	public static string MISSING_COMPONENT_NAME = "O nome do componente referenciado não existe. Cheque no inspector se o nome do elemento bate com o nome definido em <Constantes.cs>";
	public static string NO_TAG_MATCHED = "No tag matched";
	public static string CANNOT_LOAD_PATH = "Não foi possível carregar os arquivos no path especificado. Cheque o path em <Constantes.cs> e compare com a localizacao no inspector.";
	public static string MISSING_AMMO_HANDLER_SCRIPT = "Voce colidiu com um projétil, logo deveria haver um script <AmmoHandler> associado! Certifique que voce o colocou no inspector!";
	public static string MISSING_REF = "Referencia não existe, cheque no inspector...";
	public static string ASSING_SOMETHING = "Voce precisa escolher algum opcao! Veja no inspector";
	public static string OBJECT_HANDLER_SCRIPT_REQUIRED = "O objeto nao tem o script ObjectHandler, certifique-se de que ele tenha!";
	//LAYERS
	public static int PLAYER_PROJECTIL_LAYER = 10;
	public static int ENEMY_LAYER = 9;
	//PLAYER AMMO TYPES: PODE SER LIENAR, DIAGONAL, PERSEGUIDORA
	public static string TRACKER = "Tracker";
	public static string LINEAR = "Linear";
	public static string DIAGONAL = "Diagonal";
	//PAIENL QUE SERÁ FADADO PRA SIMULAR MUDANCA DE CENA DE FORMA LEGAL
	public static string FADER_PANEL = "FaderPanel";
	
}