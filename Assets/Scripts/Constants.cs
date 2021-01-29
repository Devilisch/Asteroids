static class Constants {
    // screen size parameters
    public const  float    SCREEN_TOP_SIDE       = 5.5f;
    public const  float    SCREEN_BOTTOM_SIDE    = -5.5f;
    public const  float    SCREEN_RIGHT_SIDE     = 7f;
    public const  float    SCREEN_LEFT_SIDE      = -7f;
    public const  float    RESPAWN_SPACE         = 3f;

    // asteroids parameters
    public const  int      MAX_ASTEROID_STATES   = 4;
    public static string[] ASTEROID_STATE_NAMES  = new string[MAX_ASTEROID_STATES] { "Big", "Middle", "Small", "Pixel"};
    public static float[]  ASTEROID_STATE_SCALES = new float [MAX_ASTEROID_STATES] { 1f,    0.6f,     0.2f,    0.1f};
    public static int[]    ASTEROID_STATE_MASS   = new int   [MAX_ASTEROID_STATES] { 100,   60,       20,      10};
    public const  int      MAX_ASTEROID_CHUNKS   = 2;

    // bullet parameters
    public const  float    BULLET_FORCE          = 1000f;
    public const  float    BULLET_LIFE_TIME      = 3.0f;
    public const  float    BULLET_SCALE          = 0.5f;
}
