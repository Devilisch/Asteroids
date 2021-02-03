static class Constants {
    // screen size parameters
    public const  float    SCREEN_TOP_SIDE             = 5.5f;
    public const  float    SCREEN_BOTTOM_SIDE          = -5.5f;
    public const  float    SCREEN_RIGHT_SIDE           = 7f;
    public const  float    SCREEN_LEFT_SIDE            = -7f;
    public const  float    RESPAWN_SPACE               = 3f;

    // asteroids parameters
    public const  int      MAX_ASTEROID_STATES         = 4;
    public static string[] ASTEROID_STATE_NAMES        = new string[MAX_ASTEROID_STATES] { "Big", "Middle", "Small", "Pixel"};
    public static float[]  ASTEROID_STATE_SCALES       = new float [MAX_ASTEROID_STATES] { 1f,    1f,     1f,    1f};
    public static int[]    ASTEROID_STATE_MASS         = new int   [MAX_ASTEROID_STATES] { 100,   80,       40,      20};
    public const  float    MAX_ASTEROID_FORCE          = 5000f;
    public const  float    MAX_ASTEROID_TORQUE         = 1000f;
    public const  int      MIN_ASTEROID_CHUNKS         = 1;
    public const  int      MAX_ASTEROID_CHUNKS         = 2;
    public const  float    SPACE_BETWEEN_CHUNKS        = 1.5f;
    public const  int      MIN_ASTEROIDS_ON_SCREEN     = 2;
    public const  int      MAX_ASTEROIDS_ON_SCREEN     = 10;

    // bullet parameters
    public const  float    BULLET_FORCE                = 1000f;
    public const  float    BULLET_LIFE_TIME            = 3.0f;
    public const  float    BULLET_SCALE                = 1f;

    // score parameters
    public const  int      POINTS_PER_ASTEROID_STATE   = 25;
    public const  float    DANGER_ZONE_ASTEROID_RADIUS = 1.5f;
    public const  int      DANGER_ZONE_ASTEROID_POINTS = 10;
    public const  int      POINTS_PER_UFO              = 100;
    public const  float    DANGER_ZONE_UFO_RADIUS      = 2f;
    public const  int      DANGER_ZONE_UFO_POINTS      = 50;

    // UFO parameters
    public const  float    UFO_SCALE                   = 1f;
    public const  int      UFO_MASS                    = 30;
    public const  float    UFO_FORCE                   = 3000f;
    public const  float    UFO_AGRO_ZONE               = 6f;
    public const  float    UFO_AGRO_FORCE              = 1.5f;

    // event parameters
    public const  float    UFO_RESPAWN_COOLDOWN        = 5f;
    public const  float    ASTEROID_RESPAWN_COOLDOWN   = 13f;
    public const  int      PLAYER_LIVES                = 3;
    public const  string   LIVE_SYMBOL                 = "♥";

    // theme names
    public const  string   THEME_90S                   = "90s_theme";
    public const  string   THEME_PIXEL                 = "pixel_theme"; // not in game

    // other
    public const  string   HIGHSCORE_PATH              = "Assets/Resources/Highscore.txt";
    public const  string   MAIN_SCENE                  = "MainScene";
    public const  string   MENU_SCENE                  = "MenuScene";
    public const  float    PLAYER_IMMORTAL_TIME        = 3f;

}
