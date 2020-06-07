using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AI_car : MonoBehaviour {

	int gen = 0,pop = 15,carpan = 0;

	int cikisSayisi = 116;

	float mutasyon = 0.03f;
	float baslangicZaman;
	float skor = -1;


	GameObject arac,baslangic,hareket,carpanlar,bestArac;
	private GeneticAlgorithm<float> ga;
	private System.Random random;

	void Start () {
		random = new System.Random();
		ga = new GeneticAlgorithm<float>(pop, cikisSayisi, random, rastgeleSayiAl, skorHesapla, 2, mutasyon);

		arac = GameObject.Find ("aracPrefab3");
		baslangic = GameObject.Find ("baslangic");
		hareket = GameObject.Find ("hareket");
		carpanlar = GameObject.Find ("carpanlar");
		agEgit ();
	}

	private float rastgeleSayiAl() {
		return (float)((2*random.NextDouble()) -1);
	}

	float skorHesapla(int num) {
		return GameObject.Find("arac" + num.ToString()).GetComponent<aracKod>().skor;
	}
	

	void Update () {
		/*
		if(((Time.time - baslangicZaman) > 50) ) {
			carpan = 0;
			gen++;
			skor = -1;
			agEgit ();
		}
		*/

	}
	public void agEgit() {
		

		Debug.Log ("jenerasyon:" + gen);
		
		if (gen == 0) {
			carpan = 0;
			for (int i = 0; i < pop; i++) {
				GameObject clone = Instantiate(arac,baslangic.transform.position,arac.transform.rotation);
				clone.name = "arac" + i.ToString();
				aracKod aracKodu = clone.AddComponent<aracKod>();
				aracKodu.aracDna = ga.Population.ToArray()[i];
				aracKodu.katmanAta();
				clone.transform.SetParent (hareket.transform);
				aracKodu.carpma = 0;
			}
		} 
		else {
			
			carpan = 0;

			if(hareket.transform.childCount > 0) {
				foreach(Transform tf in hareket.transform) {
					tf.GetComponent<aracKod>().carpma = 1;
					tf.GetComponent<aracKod>().skorHesapla();
					tf.SetParent(carpanlar.transform);
				}
			}


			ga.NewGeneration(3,true);

						foreach(Transform tf in carpanlar.transform) {
				Destroy(tf.gameObject);
			}

			GameObject.Find("Canvas/fitText").GetComponent<Text>().text = "Fitness: " + ga.BestFitness.ToString();
			GameObject.Find("Canvas/popText").GetComponent<Text>().text = "Populasyon: " + pop;
			GameObject.Find("Canvas/genText").GetComponent<Text>().text = "Jenerasyon: " + gen;
			GameObject.Find("Canvas/mutText").GetComponent<Text>().text = "Mutasyon: " + mutasyon;
			for(int i = 0; i < pop; i++) {
				GameObject clone2 = Instantiate(arac,baslangic.transform.position,arac.transform.rotation);
				clone2.name = "arac" + i.ToString();
				aracKod aracKodu = clone2.AddComponent<aracKod>();
				aracKodu.aracDna = ga.Population.ToArray()[i];
				aracKodu.katmanAta();
				aracKodu.carpma = 0;
				clone2.transform.SetParent (hareket.transform);
			}


		}

	
	}



	public void carpanArac() {
		carpan = carpan + 1;

		if(carpan == pop ) {
			StartCoroutine(bekle());
		}
	}


	IEnumerator bekle() {
		baslangicZaman = Time.time;
		yield return new WaitForSeconds(5);
		if(carpan != 0) {
			foreach(Transform tf in carpanlar.transform) {
				aracKod tmpKod = tf.GetComponent<aracKod>();
				if(tmpKod.skor > skor) {
					skor = tmpKod.skor;
					bestArac = tf.gameObject;
				}
			}



			carpan = 0;
			gen++;
			skor= -1;
			agEgit ();
		}

	}

/*
	private void UpdateText(char[] bestGenes, float bestFitness, int generation, int populationSize, Func<int, char[]> getGenes)
	{
		bestText.text = CharArrayToString(bestGenes);
		bestFitnessText.text = bestFitness.ToString();

		numGenerationsText.text = generation.ToString();

		for (int i = 0; i < textList.Count; i++)
		{
			var sb = new StringBuilder();
			int endIndex = i == textList.Count - 1 ? populationSize : (i + 1) * numCharsPerTextObj;
			for (int j = i * numCharsPerTextObj; j < endIndex; j++)
			{
				foreach (var c in getGenes(j))
				{
					sb.Append(c);
				}
				if (j < endIndex - 1) sb.AppendLine();
			}

			textList[i].text = sb.ToString();
		}
	}

	private string CharArrayToString(char[] charArray)
	{
		var sb = new StringBuilder();
		foreach (var c in charArray)
		{
			sb.Append(c);
		}

		return sb.ToString();
	}
*/


}
