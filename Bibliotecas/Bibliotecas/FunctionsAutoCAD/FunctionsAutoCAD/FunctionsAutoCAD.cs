using Autodesk.AutoCAD.Interop;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Autodesk.AutoCAD.Interop.Common;
using System;

namespace FunctionsAutoCAD
{
    /// <summary>
    /// Tipo de punta de la cota.
    /// </summary>
    public enum ArrowHeadType
    {
        /// <summary>
        /// Punta en circulo.
        /// </summary>
        ArrowDot,
        /// <summary>
        /// Punta en flecha.
        /// </summary>
        ArrowDefault


    }


    /// <summary>
    /// 
    /// </summary>
    public enum PrecisionCota
    {
       /// <summary>
       /// 
       /// </summary>
       Cero,
       /// <summary>
        /// 
        /// </summary>
        Uno,
       /// <summary>
        /// 
        /// </summary>
        Dos,
        /// <summary>
        /// 
        /// </summary>
        Tres,
        /// <summary>
        /// 
        /// </summary>
        Cuatro,
        /// <summary>
        /// 
        /// </summary>
        Cinco,
        /// <summary>
        /// 
        /// </summary>
        Seis

    }

    /// <summary>
    /// Justificar Texto.
    /// </summary>
    public enum JustifyText
    {
        /// <summary>
        /// Centro
        /// </summary>
        Center,
        /// <summary>
        /// Izquierda.
        /// </summary>
        Left,
        /// <summary>
        /// Derecha.
        /// </summary>
        Right
    }


    /// <summary>
    /// Funciones basicas para la manipulación de AutoCAD - efe Prima Ce, Por: José Luis Rosales Meza
    /// </summary>
    public class FunctionsAutoCAD
    {

        private static AcadApplication AcadApp;
        private static AcadDocument AcadDoc;
       
        /// <summary>
        /// 
        /// </summary>
        public static void OpenAutoCAD()
        {
            try
            {

                AcadApp = (AcadApplication)Marshal.GetActiveObject("Autocad.Application");
                AcadDoc = AcadApp.ActiveDocument;
            }
            catch
            {
                AcadApp = new AcadApplication();
                AcadApp.Visible = true;
                OpenFileDialog openFileDialog = new OpenFileDialog()
                { Multiselect = true, Filter = "Abrir Plantilla|*.dwg", Title = "Archivo dwg" };
                openFileDialog.ShowDialog();

                if (openFileDialog != null)
                {
                    AcadDoc = AcadApp.Documents.Add(openFileDialog.FileName);
                }
            }

        }


        /// <summary>
        /// Establece escala especificada al documento actual.
        /// </summary>
        /// <param name="Scale">Escala especificada, ejemplo: "1:25"</param>
        public static void SetScale(string Scale)
        {
            if (AcadDoc != null)
            {
                AcadDoc.SetVariable("CANNOSCALE", Scale);
            }

        }

        /// <summary>
        /// Elimina una seleccion del documento actual.
        /// </summary>
        /// <param name="Selection">Selección específica a eliminar.</param>
        private static void DeleteESelec(string Selection)
        {

            foreach (AcadSelectionSet selectionSet in AcadDoc.SelectionSets)
            {
                if (selectionSet.Name == Selection)
                {
                    selectionSet.Delete();
                    break;
                }

            }


        }

        /// <summary>
        /// Agregar nueva selección al documento actual.
        /// </summary>
        /// <param name="SelectionS">Selección a agregar.</param>
        public static object AddSelection(string SelectionS)
        {
            AcadSelectionSet Selection = null;

            if (AcadDoc != null)
            {
                DeleteESelec(SelectionS);
                AcadDoc.PurgeAll();

                Selection = AcadDoc.SelectionSets.Add(SelectionS);
                Selection.SelectOnScreen();
            }
            return Selection;
        }


        /// <summary>
        /// Obtiene un punto del documento actual. 
        /// </summary>
        /// <param name="Msg">Mensaje a mostrar en el documento actual.</param>
        /// <param name="Array">Coordenadas del punto.</param>
        public static void GetPoint(ref double[] Array, string Msg = "Posicionar")
        {
            if (AcadDoc != null)
            {
                try
                {
                    Array = AcadDoc.Utility.GetPoint(Prompt: Msg);
                }
                catch { }
            }
        }


        /// <summary>
        /// Agrega un nuevo texto al documento actual.
        /// </summary>
        /// <param name="TextString">Texto a mostrar</param>
        /// <param name="P_XYZ">Coordenadas del texto (X,Y,Z).</param>
        /// <param name="Width">Ancho del texto.</param>
        ///  <param name="Height">Alto del texto.</param>
        /// <param name="Layer">Capa del texto.</param>
        /// <param name="Style">Estilo del texto.</param>
        /// <param name="Rotation">Ángulo de rotación del texto en grados.</param>
        /// <param name="LineTypeSacale">Tipo de escala de la linea.</param>
        /// <param name="Width2">Ancho del cuadro de Texto.</param>
        /// <param name="justifyText">Justificación del Texto.</param>

        public static void AddText(string TextString, double[] P_XYZ, double Width, double Height, string Layer, string Style, float Rotation, double LineTypeSacale = 1, double Width2 = 1.3,JustifyText justifyText = JustifyText.Left)
        {

            if (AcadDoc != null)
            {

                AcadMText text = AcadDoc.ModelSpace.AddMText(P_XYZ, Width, TextString);
                text.Layer = Layer;
                text.StyleName = Style;
                text.Height = Height;
                text.Rotation = Rotation * Math.PI / 180;
                text.LinetypeScale = LineTypeSacale;
                text.Width = Width2;
                text.AttachmentPoint = Clasf_JustyText(justifyText);
                text.Update();



            }

        }

        /// <summary>
        /// Añadir nueva cota al documento actual.
        /// </summary>
        /// <param name="P1_XYZ">Punto Inicial (X,Y,Z)</param>
        /// <param name="P2_XYZ">Punto Final (X,Y,Z)</param>
        /// <param name="Layer">Capa de la cota.</param>
        /// <param name="Style">Estilo de la cota.</param>
        /// <param name="DesplazCota">Desplazamiento de la Cota perpendicularmente a la línea formada.</param>
        /// <param name="DeplazaTextX">Desplazamiento en X del texto asociado a la cota</param>
        /// <param name="DeplazaTextY">Desplazamiento en Y del texto asociado a la cota</param>
        /// <param name="Precision">Cantidad de decimales de la cota. Ejemplo: (0,0.0,0.00,0.000)</param>
        /// <param name="Text">Cambia la medición original al texto establecido.</param>
        /// <param name="TextHeight">Alto del texto.</param>
        /// <param name="ArrowheadSize">Tamaño de las puntas de la cota.</param>
        /// <param name="headType1">Tipo de cabeza de la cota en la punta inicial.</param>
        /// <param name="headType2">Tipo de cabeza de la cota en la punta final.</param>
        public static void AddCota(double[] P1_XYZ, double[] P2_XYZ, string Layer, string Style, float DesplazCota = 0, float DeplazaTextX = 0, float DeplazaTextY = 0, PrecisionCota Precision = PrecisionCota.Dos, string Text = "", double TextHeight = 0.0015, 
            double ArrowheadSize = 0.001,ArrowHeadType headType1 = ArrowHeadType.ArrowDot, ArrowHeadType headType2= ArrowHeadType.ArrowDot)
        {


            if (AcadDoc != null && P1_XYZ.Length == 3 && P2_XYZ.Length == 3)
            {

                double Rotation_Rad;
                double X = P2_XYZ[0] - P1_XYZ[0];
                double Y = P2_XYZ[1] - P1_XYZ[1];
                //Encontrar Angulo 
                Rotation_Rad = Math.Atan(Y / X);
                double DesX = (Distancia(P1_XYZ, P2_XYZ) / 2) * Math.Cos(Rotation_Rad);
                double DesY = (Distancia(P1_XYZ, P2_XYZ) / 2) * Math.Sin(Rotation_Rad);
                double DesplazarXCota = DesplazCota * Math.Sin(Rotation_Rad);
                double DesplazarYCota = DesplazCota * Math.Cos(Rotation_Rad);


                double[] LocationText = new double[] { P1_XYZ[0] + DesX + DesplazarXCota, P1_XYZ[1] + DesY + DesplazarYCota, P1_XYZ[2] + (P2_XYZ[2] - P1_XYZ[2]) / 2 };
                double[] TextPosition = new double[] { LocationText[0] + DeplazaTextX, LocationText[1] + DeplazaTextY, LocationText[2] };

                AcadDimRotated cota = AcadDoc.ModelSpace.AddDimRotated(P1_XYZ, P2_XYZ, LocationText, Rotation_Rad);

                //Cotas siempre con Puntas Redondas
                cota.Arrowhead1Type = Clasf_ArrowHeadType(headType1);
                cota.Arrowhead2Type = Clasf_ArrowHeadType(headType2);
                cota.Layer = Layer;
                cota.StyleName = Layer;
                cota.TextStyle = Style;
                cota.TextHeight = TextHeight;
                cota.ArrowheadSize = ArrowheadSize;
                cota.PrimaryUnitsPrecision = Clasf_precision(Precision);
                cota.TextPosition = TextPosition;
                if (Text != "")
                {
                    cota.TextOverride = Text;

                }

                cota.Update();
            }



        }

        /// <summary>
        /// Crear un nuevo Polígono en 2D.
        /// </summary>
        /// <param name="VerticesList">Vertices del polígono. Ejemplo: (X1,Y1, X2,Y2, X3,Y3 X4,Y4)</param>
        /// <param name="Layer">Capa del Polígono</param>
        /// <param name="Closed">Establece si el polígono es cerrado</param>
        public static void AddPolyline2D(double[] VerticesList, string Layer, bool Closed = true)
        {
            if (AcadDoc != null)
            {
                AcadLWPolyline polyline = AcadDoc.ModelSpace.AddLightWeightPolyline(VerticesList);
                polyline.Layer = Layer;
                polyline.Closed = Closed;
                polyline.Update();
            }
        }
        /// <summary>
        /// Crear un nuevo Polígono en 2D con su respectivo Relleno (Hatch).
        /// </summary>
        /// <param name="VerticesList">Vértices del Polígono</param>
        /// <param name="Layer">Capa del Polígono.</param>
        /// <param name="Pattern">Tipo de Hatch (SOILD,ANGLE,ANS31,...)</param>
        /// <param name="LayerHatch">Capa del Hatch</param>
        /// <param name="ScaleHacth">Escala del Hatch</param>
        /// <param name="PatternScale">Factor de escala del tipo de Hatch</param>
        /// <param name="PatternAngle">Angulo del tipo de Hatch</param>
        public static void AddPolyline2D(double[] VerticesList, string Layer, string Pattern, string LayerHatch, double ScaleHacth, double PatternScale = 0.009, float PatternAngle = 45)
        {
            if (AcadDoc != null)
            {
                AcadLWPolyline polyline = AcadDoc.ModelSpace.AddLightWeightPolyline(VerticesList);
                polyline.Layer = Layer;
                polyline.Closed = true;
                AddHatch((AcadEntity)polyline, Pattern, LayerHatch, ScaleHacth, PatternScale, PatternAngle);
                polyline.Update();
            }

        }


        /// <summary>
        ///  Añadir un nuevo Poligono y un texto representado el largo de este.
        /// </summary>
        /// <param name="VerticesPolyline">Vertices del polígono. Ejemplo: (X1,Y1, X2,Y2, X3,Y3 X4,Y4)</param>
        /// <param name="LayerPolyline">Capa del Polígono</param>
        /// <param name="TextString">Texto adicional al largo del Polígono</param>
        /// <param name="PText_XYZ">Coordenadas del texto (X,Y,Z).</param>
        /// <param name="Width">Alto del Texto.</param>
        /// <param name="Height">Ancho del Texto.</param>
        /// <param name="LayerText">Capa del Texto.</param>
        /// <param name="StyleText">Estilo del Texto</param>
        /// <param name="Rotation">Ángulo de rotación del texto en grados </param>
        /// <param name="Width2">Ancho del cuadro de Texto.</param>

        public static void AddPolyline2DWithLengthText(double[] VerticesPolyline, string LayerPolyline, string TextString, double[] PText_XYZ, double Width, double Height, string LayerText, string StyleText, float Rotation,double Width2 = 1.3)
        {
            if (AcadDoc != null)
            {
                AcadLWPolyline polyline = AcadDoc.ModelSpace.AddLightWeightPolyline(VerticesPolyline);
                polyline.Layer = LayerPolyline;
                polyline.Update();
                TextString += @"%<\AcObjProp Object(%<\_ObjId " + polyline.ObjectID + @">%).Length \f " + (char)(34) + "%lu2%pr2" + (char)(34) + ">%";
                AddText(TextString, PText_XYZ, Width, Height, LayerText, StyleText, Rotation,Width2:Width2);


            }
        }

        //BLOQUES ESPECIFICOS DE EFE PRIMA CE 

        /// <summary>
        /// Bloque: Nivel de Sección - Efe Prima Ce
        /// </summary>
        /// <param name="P_XYZ">Coordenadas del Bloque.</param>
        /// <param name="Nivel">Texto correspondiente al nivel especificado, se recomienda asignar el texto así:  N+/PLosa xx, “/P” es para agregar un espacio.</param>
        /// <param name="Distance1">Largo de la línea superior del bloque, en Escalas 1:75 se recomienda: 1.08 </param>
        /// <param name="Distance2">Largo de la línea inferior del bloque medido desde el <paramref name="P_XYZ"/>, en Escalas 1:75 se recomienda: 1.90</param>
        /// <param name="FlipSate1">Dirección del Bloque, False: Izquierda, True: Derecha</param>
        /// <param name="Layer">Capa del Bloque.</param>
        /// <param name="Xscale">Escala en X del Bloque.</param>
        /// <param name="Yscale">Escala en Y del Bloque.</param>
        /// <param name="Zscale">Escala en Z del Bloque.</param>
        /// <param name="Rotation">Ángulo de rotación en grados del Bloque.</param>

        public static void B_NivelSeccion(double[] P_XYZ, string Nivel, double Distance1, double Distance2, bool FlipSate1, string Layer, double Xscale, double Yscale, double Zscale, float Rotation)
        {

            if (AcadDoc != null)
            {

                AcadBlockReference blockReference = AcadDoc.ModelSpace.InsertBlock(P_XYZ, "FC_B_Nivel seccion", Xscale, Yscale, Zscale, Rotation);
                blockReference.Layer = Layer;



                var referenceProperty = blockReference.GetDynamicBlockProperties();
                var attributeReference = blockReference.GetAttributes();


                referenceProperty[0].Value = Distance1;
                referenceProperty[2].Value = Distance2;

                if (FlipSate1 == true)
                {
                    referenceProperty[4].Value = Convert.ToInt16(1);
                }

                attributeReference[0].TextString = Nivel;
                blockReference.Update();
            }




        }

        /// <summary>
        /// Bloque: Corte de Sección - Efe Prima Ce
        /// </summary>
        /// <param name="P_XYZ">Coordenadas del Bloque</param>
        /// <param name="X1">Texto del corte, punta inicial.</param>
        /// <param name="X2">Texto del corte, punta final.</param>
        /// <param name="Distance1">Distancia medida desde el <paramref name="P_XYZ"/>, usualmente se asigna el espesor del elemento hacer el corte.</param>
        /// <param name="Position1X">Posición en X del Texto ubicado en la punta inicial, se recomienda en Columnas,Escala 1:75 0.1.</param>
        /// <param name="Position1Y">Posición en Y del Texto ubicado en la punta inicial, se recomienda en Columnas,Escala 1:75 -0.1688.</param>
        /// <param name="Position2X">Posición en X del Texto ubicado en la punta final, se recomienda en Columnas,Escala 1:75 <paramref name="Distance1"/> + 1.0  - <paramref name="Position1X"/></param>
        /// <param name="Position2Y">Posición en Y del Texto ubicado en la punta final, se recomienda en Columnas,Escala 1:75 -0.1688.</param>
        /// <param name="FlipSate1">Dirección del Bloque: True: Abajo, False: Arriba</param>
        /// <param name="Layer">Capa del Bloque.</param>
        /// <param name="Xscale">Escala en X del Bloque.</param>
        /// <param name="Yscale">Escala en Y del Bloque.</param>
        /// <param name="Zscale">Escala en Z del Bloque.</param>
        /// <param name="Rotation">Ángulo de rotación en grados del Bloque.</param>
        public static void B_Seccion(double[] P_XYZ, string X1,string X2, double Distance1, double Position1X, double Position2X, double Position1Y, double Position2Y, bool FlipSate1, string Layer, double Xscale, double Yscale, double Zscale, float Rotation)
        {

            if (AcadDoc != null)
            {

                AcadBlockReference blockReference = AcadDoc.ModelSpace.InsertBlock(P_XYZ, "FC_B_Seccion", Xscale, Yscale, Zscale, Rotation);
                blockReference.Layer = Layer;

                var referenceProperty = blockReference.GetDynamicBlockProperties();
                var attributeReference = blockReference.GetAttributes();

                referenceProperty[0].Value = Position1X;
                referenceProperty[1].Value = Position1Y;
                referenceProperty[4].Value = Distance1;
                referenceProperty[2].Value = Position2X;
                referenceProperty[3].Value = Position2Y;
              


                if (FlipSate1 == true)
                {
                    referenceProperty[6].Value = Convert.ToInt16(1);
                }

                attributeReference[0].TextString = X1;
                attributeReference[1].TextString = X2;
                blockReference.Update();
            }


        }

        







        private static void AddHatch(AcadEntity entity, string Pattern, string LayerHatch, double ScaleHacth, double PatternScale, double PatternAngle)
        {

            AcadHatch hatch = AcadDoc.ModelSpace.AddHatch(0, Pattern, true);
            AcadEntity[] entities = new AcadEntity[] { entity };
            hatch.AppendInnerLoop(entities);
            hatch.Layer = LayerHatch;
            hatch.LinetypeScale = ScaleHacth;
            hatch.PatternAngle = PatternAngle;
            BackHatch((AcadObject)hatch);
            hatch.Update();

        }
        private static void BackHatch(AcadObject AcadObject)
        {
            AcadDictionary dictionary = AcadDoc.ModelSpace.GetExtensionDictionary();
            AcadSortentsTable sentityObj = (AcadSortentsTable)dictionary.GetObject("ACAD_SORTENTS");
            AcadObject[] entities = new AcadObject[] { AcadObject };
            sentityObj.MoveToBottom(entities);

        }


        private static AcDimPrecision Clasf_precision(PrecisionCota presicionCota)
        {


            if (presicionCota == PrecisionCota.Cero)
            { return AcDimPrecision.acDimPrecisionZero; }
            else if (presicionCota == PrecisionCota.Uno)
            { return AcDimPrecision.acDimPrecisionOne; }
            else if (presicionCota == PrecisionCota.Dos)
            { return AcDimPrecision.acDimPrecisionTwo; }
            else if (presicionCota == PrecisionCota.Tres)
            { return AcDimPrecision.acDimPrecisionThree; }
            else if (presicionCota == PrecisionCota.Cuatro)
            { return AcDimPrecision.acDimPrecisionFour; }
            else if (presicionCota == PrecisionCota.Cinco)
            { return AcDimPrecision.acDimPrecisionFive; }
            else if (presicionCota == PrecisionCota.Seis)
            { return AcDimPrecision.acDimPrecisionSix; }
            else { return AcDimPrecision.acDimPrecisionTwo; }

        }


        private static AcAttachmentPoint Clasf_JustyText(JustifyText justify)
        {
            if(justify == JustifyText.Center)
            {
                return AcAttachmentPoint.acAttachmentPointMiddleCenter;
            }
            else if( justify == JustifyText.Right){
                return AcAttachmentPoint.acAttachmentPointBottomRight;
            }else
            {
                return AcAttachmentPoint.acAttachmentPointBottomLeft;
            }
        }


        private static AcDimArrowheadType Clasf_ArrowHeadType(ArrowHeadType headType)
        {
            if(headType == ArrowHeadType.ArrowDefault)
            {
                return AcDimArrowheadType.acArrowDefault;
            }
            else if(headType == ArrowHeadType.ArrowDot)
            {
                return AcDimArrowheadType.acArrowDot;
            }
            else
            {
                return AcDimArrowheadType.acArrowDot;
            }

        }

        private static double Distancia(double[] P1_XYZ, double[] P2_XYZ)
        {
            if (P1_XYZ.Length == 3 & P2_XYZ.Length == 3)
            {
                return Math.Sqrt(Math.Pow(P1_XYZ[0] - P2_XYZ[0], 2) + Math.Pow(P1_XYZ[1] - P2_XYZ[1], 2) + Math.Pow(P1_XYZ[2] - P2_XYZ[2], 2));
            }
            else
            {
                return 0;
            }


        }



    }
}
