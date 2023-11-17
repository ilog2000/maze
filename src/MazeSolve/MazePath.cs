public class MazePath
{
    public List<PathPoint> Points { get; } = new List<PathPoint>();
    public bool Solved { get; set; }

    public override bool Equals(object obj)
    {
        if (obj is MazePath other)
        {
            if (Points.Count != other.Points.Count)
                return false;

            for (int i = 0; i < Points.Count; i++)
            {
                if (Points[i].X != other.Points[i].X || Points[i].Y != other.Points[i].Y)
                    return false;
            }

            return true;
        }
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}