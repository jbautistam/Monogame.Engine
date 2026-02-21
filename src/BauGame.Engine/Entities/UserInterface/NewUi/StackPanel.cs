namespace MonoGame.UI
{
	/// <summary>
	/// Contenedor con layout automático para hijos
	/// </summary>
	public class StackPanel : Panel
    {
        public enum Orientation { Vertical, Horizontal }
        
        public Orientation StackOrientation { get; set; } = Orientation.Vertical;
        public float Spacing { get; set; } = 0.02f; // Espacio entre elementos en coordenadas relativas
        public bool AutoSize { get; set; } = false; // Ajusta tamaño al contenido

        protected override void CalculateTransformations()
        {
            base.CalculateTransformations();
            
            if (!AutoSize || Children.Count == 0) return;

            // Recalcula posiciones de hijos según orientación
            float currentPos = 0f;
            float availableSpace = StackOrientation == Orientation.Vertical 
                ? 1f - Padding.Vertical 
                : 1f - Padding.Horizontal;
            
            float elementSize = (availableSpace - (Spacing * (Children.Count - 1))) / Children.Count;

            foreach (var child in Children)
            {
                if (StackOrientation == Orientation.Vertical)
                {
                    child.RelativeBounds = new RectangleRelative(
                        Padding.Left,
                        Padding.Top + currentPos,
                        1f - Padding.Horizontal,
                        elementSize
                    );
                    currentPos += elementSize + Spacing;
                }
                else
                {
                    child.RelativeBounds = new RectangleRelative(
                        Padding.Left + currentPos,
                        Padding.Top,
                        elementSize,
                        1f - Padding.Vertical
                    );
                    currentPos += elementSize + Spacing;
                }
                
                child.Invalidate();
            }
        }
    }
}
