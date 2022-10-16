public enum Direction
{
    None = -1,
    Left,
    Right,
    Up,
    Down
}

public enum GameState
{
    StartScene,
    GameScene
}

public enum Legend
{
    Null = -1,
    Empty,
    OutsideCorner,
    OutsideWall,
    InsideCorner,
    InsideWall,
    StandardPellet,
    PowerPellet,
    TJunction
}

public enum MusicClips
{
    Intro,
    Calm,
    Scared,
    Dead
}

public enum PacStudentState
{
    Null = -1,
    Walking,
    EatingPellet,
    HitWall,
    Death
}

public enum Points : int
{
    None = 0,
    StandardPellet = 10,
    BonusCherry = 100
}