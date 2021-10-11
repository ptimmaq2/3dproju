using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretEnemy : MonoBehaviour
{

    public GameObject targetLocation; //kiekko
    public GameObject ammoSpawn; //empty, josta ammus l‰htee
    public GameObject ammo; //ammo prefabs kansiosta
    public GameObject gunRotator; //empty joka k‰‰nt‰‰ asetta

    public float force; // voimakkuus jolla ammutaan. Esim. 27.
    public Vector3 gravity; //painovoima
    private int angleMultiplier;


    // Start is called before the first frame update
    void Start()
    {
        gravity = Physics.gravity;   
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.LookAt(new Vector3(targetLocation.transform.position.x,
            gameObject.transform.position.y, targetLocation.transform.position.z));   
    }

    public void Shoot()
    {
        //k‰ynnistet‰‰n coroutine
        StartCoroutine(ShootBalls());
    }

    IEnumerator ShootBalls()
    {
        Debug.Log("Ammutaan ammus.");
        Vector3[] direction = HitTargetBySpeed(ammoSpawn.transform.position, targetLocation.transform.position, gravity, force);

        if(gameObject.transform.position.z < targetLocation.transform.position.z)
        {
            angleMultiplier = -1;
        }
        else
        {
            angleMultiplier = 1;
        }

        //Ennen kuin ammutaan 1. ammus lasketaan kulma mihin piipun pit‰isi k‰‰nty‰.
        //Odotetaan niin kauan kunnes piippu on k‰‰ntynyt ja vasta sitten ammutaan.

        Debug.Log("Piipun tulisi k‰‰nty‰ kulmaan " + Mathf.Atan(direction[0].y / direction[0].z) * Mathf.Rad2Deg * angleMultiplier);
        gunRotator.GetComponent<RotateGun>().xAngle = Mathf.Atan(direction[0].y / direction[0].z) * Mathf.Rad2Deg * angleMultiplier;

        //Pys‰yt‰ ajo niin pitk‰ksi aikaa kunnes k‰‰ntyminen on p‰‰ttynyt. K‰‰ntyminen on p‰‰ttynyt kun rotating on false.
        yield return new WaitUntil(() => gunRotator.GetComponent<RotateGun>().rotating == false);


        //eka ammus
        GameObject projectile = Instantiate(ammo, ammoSpawn.transform.position, Quaternion.identity);
        projectile.GetComponent<Rigidbody>().AddRelativeForce(direction[0], ForceMode.Impulse);

        //2s viive ammuksien v‰lill‰.
        yield return new WaitForSeconds(2);

        //Samat tokalle ammukselle.
        Debug.Log("Piipun tulisi k‰‰nty‰ kulmaan " + Mathf.Atan(direction[1].y / direction[1].z) * Mathf.Rad2Deg * angleMultiplier);
        gunRotator.GetComponent<RotateGun>().xAngle = Mathf.Atan(direction[1].y / direction[1].z) * Mathf.Rad2Deg * angleMultiplier;
        yield return new WaitUntil(() => gunRotator.GetComponent<RotateGun>().rotating == false);


        //toka ammus
        GameObject projectile2 = Instantiate(ammo, ammoSpawn.transform.position, Quaternion.identity);
        projectile2.GetComponent<Rigidbody>().AddRelativeForce(direction[1], ForceMode.Impulse);

    }


    //c# on listoja ja taulukoita.
    //int[] numerotaulukko = new int[5] olisi taulukko numeroita,
    //numerotaulukko[3] = 1; olisi ett‰ lis‰t‰‰n 3. kohtaan arvo 1.
    //taulukon kokoa ei voi kasvattaa j‰lkik‰teen. pit‰‰ tehd‰ uusi isompi ja kopioida edellisen arvot siihen.
    //1      
    //2     
    //3     1

    //taulukko vector3[] 
    //Lista taas olisi
    //List<int> numerolista = new List<int>();
    //Listaan voi lis‰t‰ ja poistaa kaikki yms. erilaisia toimintoja mit‰ ei ole valmiina taulukoilla.
    //void funktio ei palauta mit‰‰n, voi tehd‰ public string, joka
    //palauttaisi stringin, jne. voi olla vaikka int, gameobject mik‰ vaan.

    //palauttaa taulukon vector kolmosia.
    public Vector3[] HitTargetBySpeed(Vector3 startPosition, Vector3 targetPosition, Vector3 gravityBase, float launchSpeed)
    {

        Vector3 AtoB = targetPosition - startPosition;
        Vector3 horizontal = GetHorizontalVector(AtoB, gravityBase, startPosition);
        Vector3 vertical = GetVerticalVector(AtoB, gravityBase, startPosition);
        float horizontalDistance = horizontal.magnitude;
        float verticalDistance = vertical.magnitude * Mathf.Sign(Vector3.Dot(vertical, -gravityBase));
        float x2 = horizontalDistance * horizontalDistance;
        float v2 = launchSpeed * launchSpeed;
        float v4 = launchSpeed * launchSpeed * launchSpeed * launchSpeed;
        float gravMag = gravityBase.magnitude;

        //jos launchtest on negativiinen ei ole mahdollisuutta osua kohteeseen annetulla forcella.
        //jos se on positiivinen osuminen on mahdollista ja voidaan laskea kulmat.

        float launchTest = v4 - (gravMag * ((gravMag * x2) + (2 * verticalDistance)));

        Debug.Log("Launchtest: " + launchTest);

        Vector3[] launch = new Vector3[2];

        if(launchTest < 0)
        {
            Debug.Log("Ei voida osua maaliin, ammutaan kuitenkin 45 asteen kulmassa.");

            launch[0] = (horizontal.normalized * launchSpeed * Mathf.Cos(45.0f * Mathf.Deg2Rad))
                - gravityBase.normalized * launchSpeed * Mathf.Sin(45.0f * Mathf.Deg2Rad);

            launch[1] = (horizontal.normalized * launchSpeed * Mathf.Cos(45.0f * Mathf.Deg2Rad))
              - gravityBase.normalized * launchSpeed * Mathf.Sin(45.0f * Mathf.Deg2Rad);
        }
        else
        {
            Debug.Log("Voidaan osua maaliin, lasketaan kulmat.");

            float[] tanAngle = new float[2];
            tanAngle[0] = (v2 - Mathf.Sqrt(v4 - gravMag * ((gravMag * x2) + (2 * verticalDistance * v2)))) / (gravMag * horizontalDistance);
            tanAngle[1] = (v2 + Mathf.Sqrt(v4 - gravMag * ((gravMag * x2) + (2 * verticalDistance * v2)))) / (gravMag * horizontalDistance);

            float[] finalAngle = new float[2];
            finalAngle[0] = Mathf.Atan(tanAngle[0]);
            finalAngle[1] = Mathf.Atan(tanAngle[1]);

            Debug.Log("Kulmat, joissa tykki ampuu ovat: " + finalAngle[0] * Mathf.Rad2Deg + " ja " + finalAngle[1] * Mathf.Rad2Deg);

            launch[0] = (horizontal.normalized * launchSpeed * Mathf.Cos(finalAngle[0]))
                - gravityBase.normalized * launchSpeed * Mathf.Sin(finalAngle[0]);

            launch[1] = (horizontal.normalized * launchSpeed * Mathf.Cos(finalAngle[1]))
              - gravityBase.normalized * launchSpeed * Mathf.Sin(finalAngle[1]);

        }

        return launch;

    }

    public Vector3 GetHorizontalVector(Vector3 AtoB, Vector3 gravityBase, Vector3 startPosition)
    {
        Vector3 output;
        Vector3 perpendicular = Vector3.Cross(AtoB, gravityBase);
        perpendicular = Vector3.Cross(gravityBase, perpendicular);
        output = Vector3.Project(AtoB, perpendicular);
        Debug.DrawRay(startPosition, output, Color.green, 10f);


        return output;
    }

    public Vector3 GetVerticalVector(Vector3 AtoB, Vector3 gravityBase, Vector3 startPosition)
    {
        Vector3 output;
        output = Vector3.Project(AtoB, gravityBase);
        Debug.DrawRay(startPosition, output, Color.cyan, 10f);

        return output;

    }
}
