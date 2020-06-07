using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class aracKod : MonoBehaviour {

	public int carpma = 1;
	public float skor = 0;

	public DNA<float> aracDna;
	float[] giris = new float[3];
	

	RaycastHit hitSol,hitSag,hitOn;

	GameObject cam,tmpObj;

	Vector3 forw;

	
	public string katman1 ="3;7;"; //21 + 42 + 30 + 15 + 6 + 2

	public string katman2 ="7;6;";

	public string katman3 ="6;5;";

	public string katman4 ="5;3;";

	public string katman5 ="3;2;";
	public string katman6 ="2;1;";


	

	void Start () {
	
	
		cam = GameObject.Find ("Main Camera");
		tmpObj = GameObject.Find ("carpanlar");
		//katmanlaraDegerAta(101);

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	float[] katmanCikis(string katman,float[] girisler) {

		string[] strToKatman = katman.Split(';');
		int cikisSayisi = int.Parse(strToKatman[1]);
		int katmanSayisi = int.Parse(strToKatman[0]);
		float[] tmpCikis = new float[cikisSayisi];
		for(int i = 0;i<cikisSayisi;i++) {
			tmpCikis[i] = 0;
			for(int j = 0;j<katmanSayisi;j++) {
				tmpCikis[i] += girisler[j] * float.Parse(strToKatman[((j*i) + j) + 2]);
			}
			tmpCikis[i] = aktivasyon(tmpCikis[i]);
		}
		return tmpCikis;
	}

	float aktivasyon(float sayi) {
		return sayi;
        //return (float)System.Math.Tanh(sayi);
		//return (1.0f / (1.0f + Mathf.Exp (-sayi)));
	}





	void FixedUpdate () {
		if (carpma == 0) {
			Vector3 rayPos = transform.position;
		
			Physics.Raycast (rayPos, -transform.right, out hitSol, 1000f);
			Physics.Raycast (rayPos, transform.right, out hitSag, 1000f);
			Physics.Raycast (rayPos, -transform.forward, out hitOn, 1000f);
			giris [0] = hitSag.distance;
			giris [1] = hitSol.distance;
			giris [2] = hitOn.distance;

		
			float[] cikis = katmanCikis(katman6,katmanCikis(katman5,katmanCikis(katman4,katmanCikis(katman3,katmanCikis(katman2,katmanCikis(katman1,giris))))));

		
				if(cikis[0] < 0) {
					transform.Rotate (gameObject.transform.up * 60f * Time.deltaTime,Space.World);

				}
				else {
					transform.Rotate (gameObject.transform.up * -60f * Time.deltaTime,Space.World);
				}
			
			//transform.Rotate (gameObject.transform.up * (float)System.Math.Tanh(cikis[0]) * 160f * Time.deltaTime,Space.World);
	

			transform.Translate (-transform.forward* 5 * Time.deltaTime,Space.World);
			Debug.DrawLine (transform.position, transform.position + (20 *-transform.right));
			Debug.DrawLine (transform.position, transform.position + (20 *transform.right));
			Debug.DrawLine (transform.position, transform.position + (20 *-transform.forward));
		}

	}



	void OnTriggerEnter(Collider col) {
		if (col.transform.parent.name == "parkur" && carpma == 0) {
			carpma = 1;
			transform.SetParent (tmpObj.transform);

			NavMeshPath path = new NavMeshPath();
			Vector3 tmpVec = transform.position;
			tmpVec.y = 0;
			NavMesh.CalculatePath(tmpVec, new Vector3(-36.31f,0,-55.5f), NavMesh.AllAreas, path);

			skor = 0;
			for (int i = 0; i < path.corners.Length - 1; i++) {
				skor += Vector3.Distance(path.corners[i],path.corners[i + 1]);
			}

	
			
            cam.GetComponent<AI_car> ().carpanArac();
		
			
		
		}
	}

		public void skorHesapla() {
			NavMeshPath path = new NavMeshPath();
			Vector3 tmpVec = transform.position;
			tmpVec.y = 0;
			NavMesh.CalculatePath(tmpVec, new Vector3(-17.38f,0,12.32f), NavMesh.AllAreas, path);

			skor = 0;
			for (int i = 0; i < path.corners.Length - 1; i++) {
				skor += Vector3.Distance(path.corners[i],path.corners[i + 1]);
			}
		}

	public void katmanAta() {
		katman1 = dnaOku(katman1,0,21);
		katman2 = dnaOku(katman2,21,63);
		katman3 = dnaOku(katman3,63,93);
		katman4 = dnaOku(katman4,93,108);
		katman5 = dnaOku(katman5,108,114);
		katman6 = dnaOku(katman6,114,116);
	}





	string dnaOku(string agirlik,int minGen, int maxGen) {

		string[] strAgirlik = agirlik.Split(';');
		int noron = int.Parse(agirlik.Split(';')[0]);
		int cikis = int.Parse(agirlik.Split(';')[1]);
		string tmpCikis = noron.ToString() + ";" + cikis.ToString()  + ";";

		int sayac = minGen;

	
			for(int i = 0;i<cikis;i++) {
				for(int j = 0;j<noron;j++) {
				
					tmpCikis += aracDna.Genes[sayac].ToString() + ";";
					sayac++;
				}
			}
		
	

		return tmpCikis;
	}


}



