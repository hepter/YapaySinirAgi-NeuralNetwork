using System;

namespace YapaySinirAgi
{
    internal class YSACore
    {
        public static float hataKatsayı = 0.25f;
        private Katman[] katmanlar;
        private int[] katmanŞema;

        public YSACore(int[] katmanArray)
        {
            katmanŞema = new int[katmanArray.Length];
            for (int i = 0; i < katmanArray.Length; i++)
                katmanŞema[i] = katmanArray[i];


            katmanlar = new Katman[katmanArray.Length - 1];

            for (int i = 0; i < katmanlar.Length; i++)
                katmanlar[i] = new Katman(katmanArray[i], katmanArray[i + 1]);
        }


        public float[] ForwardPropagation(float[] girisler)
        {
            katmanlar[0].forwardPropagation(girisler);
            for (int i = 1; i < katmanlar.Length; i++)
                katmanlar[i].forwardPropagation(katmanlar[i - 1].ciktilar);

            return katmanlar[katmanlar.Length - 1].ciktilar;
        }

        public void BackPropagation(float[] beklenenSonuc)
        {
            for (int i = katmanlar.Length - 1; i >= 0; i--)
            {
                if (i == katmanlar.Length - 1)
                    katmanlar[i].BackPropagationHata(beklenenSonuc);
                else
                    katmanlar[i].backPropagation(katmanlar[i + 1].hataDelta, katmanlar[i + 1].agirlik);
            }

            for (int i = 0; i < katmanlar.Length; i++)
            {
                katmanlar[i].AgirlikGuncelle();
            }
        }

        public class Katman
        {
            public static Random rnd = new Random();
            public float[,] agirlik;
            public float[,] agirlikDelta;

            public float[] ciktilar;
            public float[] girdiler;
            public float[] hataDelta;
            public float[] hatalar;
            private int toplamGirdiSayi;
            private int toplamSonucSayi;

            public Katman(int toplamGirdiSayi, int toplamSonucSayi)
            {
                this.toplamGirdiSayi = toplamGirdiSayi;
                this.toplamSonucSayi = toplamSonucSayi;
                ciktilar = new float[toplamSonucSayi];
                girdiler = new float[toplamGirdiSayi];
                agirlik = new float[toplamSonucSayi, toplamGirdiSayi];
                agirlikDelta = new float[toplamSonucSayi, toplamGirdiSayi];
                hataDelta = new float[toplamSonucSayi];
                hatalar = new float[toplamSonucSayi];

                RandomAgirlik();
            }


            public void RandomAgirlik()
            {
                for (int i = 0; i < toplamSonucSayi; i++)
                for (int j = 0; j < toplamGirdiSayi; j++)
                    agirlik[i, j] = (float) rnd.NextDouble() - 0.5f;
            }


            public float[] forwardPropagation(float[] girisler)
            {
                girdiler = girisler;


                for (int i = 0; i < toplamSonucSayi; i++)
                {
                    float NET = 0;
                    for (int j = 0; j < toplamGirdiSayi; j++)
                        NET += girisler[j] * agirlik[i, j];
                    ciktilar[i] = (float) Math.Tanh(NET); //Sigmoid(NET); 
                }

                return ciktilar;
            }


            public float Sigmoid(float deger)
            {
                double k = Math.Exp(deger);
                var sonuc = k / (1.0f + k);
                return (float) sonuc;
            }

            public void BackPropagationHata(float[] beklenen)
            {
                for (int i = 0; i < toplamSonucSayi; i++)
                    hatalar[i] = ciktilar[i] - beklenen[i];


                for (int i = 0; i < toplamSonucSayi; i++)
                    hataDelta[i] = hatalar[i] * Sigmoid(ciktilar[i]);


                for (int i = 0; i < toplamSonucSayi; i++)
                for (int j = 0; j < toplamGirdiSayi; j++)
                    agirlikDelta[i, j] = hataDelta[i] * girdiler[j];
            }

            public void backPropagation(float[] hatalar, float[,] agirliklar)
            {
                for (int i = 0; i < toplamSonucSayi; i++)
                {
                    hataDelta[i] = 0;
                    for (int j = 0; j < hatalar.Length; j++)
                        hataDelta[i] += hatalar[j] * agirliklar[j, i];

                    hataDelta[i] *= Sigmoid(ciktilar[i]);
                }

                for (int i = 0; i < toplamSonucSayi; i++)
                for (int j = 0; j < toplamGirdiSayi; j++)
                    agirlikDelta[i, j] = hataDelta[i] * girdiler[j];
            }


            public void AgirlikGuncelle()
            {
                for (int i = 0; i < toplamSonucSayi; i++)
                for (int j = 0; j < toplamGirdiSayi; j++)
                    agirlik[i, j] -= agirlikDelta[i, j] * hataKatsayı;
            }
        }
    }
}