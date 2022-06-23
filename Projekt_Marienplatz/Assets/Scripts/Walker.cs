using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
using TMPro;

public class Walker : MonoBehaviour
{
    GameObject waypointsObject;
    WayPoints waypointsScript;

    GameObject camObject;
    Cam camScript;

    GameObject player;
    Player playerScript;
    static float reachOfPlayer = 22f;
    public float distFromPlayer;


    UnityEngine.AI.NavMeshAgent agent;

    public enum Gender { Female, Male }

    // the int associated with each job is used to determine the sprite
    public enum Job { Employee = 0, Worker = 1, Unemployed = 2, Officer = 3, Farmer = 4, Capitalist = 5, Soldier = 6, Housewive = 7, Widow = 8 }
    public enum Religion { Catholic, Protestant, Jewish, Atheist }
    public enum CivilStatus { Single, Married, Widowed }
    public enum PoliticalStance { Left = 0, Liberal = 4, Conservative = 7 }
    public enum Education { Low, Middle, Academic }
    public static string[] namesFemale = { "Ursula", "Ilse", "Hildegard", "Gerda", "Ingeborg", "Irmgard", "Helga", "Gertrud", "Lieselotte", "Edith", "Erika", "Elfriede", "Gisela", "Elisabeth", "Ruth", "Anneliese", "Margarete", "Margot", "Erna", "Herta", "Maria", "Inge", "Anna", "Käthe", "Waltraud", "Ingrid", "Charlotte", "Eva", "Martha", "Else", "Irma", "Lisa", "Marianne", "Annemarie", "Frida", "Hannelore", "Karla", "Elli", "Anni", "Helene", "Lotte", "Christa", "Christel", "Hedwig", "Johanna", "Luise", "Hilde", "Wilma", "Irene", "Dorothea", "Renate", "Anita", "Marie", "Ingeburg", "Vera", "Rosemarie", "Jutta", "Elsa", "Grete", "Emma", "Rita", "Ellen", "Klara", "Thea", "Marga", "Anne", "Ella", "Emmi", "Hannah", "Elsbeth", "Lydia", "Olga", "Katharina", "Agnes", "Magda", "Brigitte", "Dora", "Paula", "Eleonore", "Hilda", "Alice", "Edeltraud", "Hella", "Berta", "Sonja", "Magdalena", "Sigrid", "Margareta", "Lilli", "Gretchen", "Rosa", "Gertraud", "Barbara", "Magdalene", "Brunhilde", "Meta", "Wally", "Traute" };
    public static string[] namesMale = { "Hans", "Günther", "Karl", "Heinz", "Werner", "Gerhard", "Walther", "Kurt", "Horst", "Helmut", "Herbert", "Ernst", "Rudolph", "Willi", "Rolf", "Erich", "Heinrich", "Otto", "Wilhelm", "Alfred", "Hermann", "Paul", "Erwin", "Wolfgang", "Klaus", "Fritz", "Friedrich", "Harald", "Franz", "Georg", "Peter", "Egon", "Bruno", "Gerd", "Harry", "Johannes", "Richard", "Jürgen", "Bernhard", "Josef", "Johann", "Joachim", "Sigfried", "Manfred", "Robert", "Albert", "Adolph", "Lothar", "Max", "Gustav", "Ewald", "Reinhold", "Martin", "Arthur", "Henri", "Karlheinz", "Edgar", "Rudi", "Ulrich", "Reinhard", "Waldemar", "Emil", "Arnold", "Diedrich", "Arno", "Eberhard", "Theodor", "Erhard", "Dieter", "Alexander", "Edmund", "Eduard", "Hugo", "Uwe", "Konrad", "Alfons", "Ludwig", "August", "Hubert", "Oskar", "Wilfried", "Anton", "Norbert", "Christian", "Gottfried", "Victor", "Fred", "Leo", "Bodo", "Michael", "Johnny", "Berthold", "Ralph", "Jakob", "Alois", "Ferdinand", "Alwin", "John", "Julius", "Jan" };
    public static Job[] JobList = { Job.Farmer, Job.Worker, Job.Employee, Job.Officer, Job.Capitalist, Job.Soldier };

    // ints associated with parties indicate political stance

    //to fix: WBWB & Zentrum werden als ein Wert behandelt weil der zugehörige int gleich ist
    public enum Party { KPD = 0, USPD = 1, MSPD = 2, DDP = 4, DVP = 5, WBWB = 6, Zentrum = 6, Buergerpartei = 7 }
    public static Party[] partyArray = { Party.KPD, Party.USPD, Party.MSPD, Party.DDP, Party.DVP, Party.Zentrum, Party.WBWB, Party.Buergerpartei };

    public string firstName;
    public Gender gender;
    public Education education;
    public int age;
    public Job job;
    public bool isWarDisabled = false;
    public Religion religion;
    public CivilStatus civilStatus;
    public float politicalStance;

    public Party[] partyAffiliationsArray;

    GameObject bodyObject;
    GameObject outlineObject;
    GameObject accessoireObject;
    SpriteRenderer bodySprite;
    SpriteRenderer outlineSprite;
    SpriteRenderer accessoireSprite;

    [SerializeField]
    Sprite[] characterSpritesMale = new Sprite[8];
    [SerializeField]
    Sprite[] characterSpritesFemale = new Sprite[8];
    [SerializeField]
    Sprite[] accessoireSprites = new Sprite[3];

    [SerializeField]
    TextAsset jsonGreetingsFile;
    public static Greetings greetings;

    TMP_Text speechBubbleText;
    SpriteRenderer speechBubbleSprite;
    
    enum InteractionState  {None, Greeted, OwningLeaflet}
    InteractionState interactionState;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        waypointsObject = GameObject.Find("WayPoints");
        waypointsScript = waypointsObject.GetComponent<WayPoints>();

        camObject = GameObject.Find("Main Camera");
        camScript = camObject.GetComponent<Cam>();

        player = GameObject.Find("Player");
        playerScript = player.GetComponent<Player>();

        agent.destination = waypointsScript.getRandomDestination();

        bodyObject = transform.Find("Body").gameObject;
        outlineObject = transform.Find("Outline").gameObject;
        accessoireObject = transform.Find("Accessoire").gameObject;
        bodySprite = bodyObject.GetComponent<SpriteRenderer>();
        outlineSprite = outlineObject.GetComponent<SpriteRenderer>();
        accessoireSprite = accessoireObject.GetComponent<SpriteRenderer>();

        string jsonGreetings = jsonGreetingsFile.ToString();
        greetings = JsonUtility.FromJson<Greetings>(jsonGreetings);

        speechBubbleText = GetComponentInChildren<TMP_Text>();
        speechBubbleSprite = transform.Find("SpeechBubble").gameObject.GetComponent<SpriteRenderer>();
        speechBubbleSprite.enabled = false;

        generateCharacterTraits();
        generateCharacterAppearance();

        interactionState = InteractionState.None;
    }

    // Update is called once per frame
    void Update()
    {
        tryInteractingWithPlayer();
    }

    public bool isAtDestination()
    {
        if( Vector2.SqrMagnitude(transform.position - agent.destination) < 5f )
        {
            return true;
        }
        return false;
    }

    public bool isUnseen()
    {
        return !camScript.isInCameraBounds(transform.position);
    }

    //This will generate normal distributed random floats given the mean and standard deviation of the distribution
    static float box_muller(float mean, float stdDev)
    {
        float u1 = 1.0f - Random.value;
        float u2 = 1.0f - Random.value;
        float randStdNorm = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) * Mathf.Sin(2.0f * Mathf.PI * u2);
        float randNorm = mean + stdDev * randStdNorm;
        return randNorm;
    }

    private void generateCharacterTraits()
    {
        // Age
        age = (int) Mathf.Clamp(Mathf.Abs( box_muller(0, 20)) + 15, 17, 79);


        // Gender
        if (Random.Range(0, 2) == 0)
        {
            gender = Gender.Female;
            firstName = getRandomElement(namesFemale);
        }
        else
        {
            gender = Gender.Male;
            firstName = getRandomElement(namesMale);
        }


        // note: all of the magic numbers used for determining the likelihood of certain attributes are just guesswork

        // Education & Job
        int educationRan = Random.Range(0, 101);
        if (gender == Gender.Female)
        {
            if (educationRan <= 70)
            {
                education = Education.Low;

                job = getRandomElement(new Job[] { Job.Unemployed, Job.Housewive, Job.Farmer, Job.Worker, Job.Employee });
            }
            else if (educationRan <= 99)
            {
                education = Education.Middle;

                job = getRandomElement(new Job[] { Job.Employee, Job.Officer, Job.Housewive });
            }
            else
            {
                education = Education.Academic;
                job = getRandomElement(new Job[] { Job.Officer });
            }
        }
        else
        {
            if(educationRan <= 70)
            {
                education = Education.Low;

                job = getRandomElement(new Job[] { Job.Unemployed, Job.Employee, Job.Farmer, Job.Soldier, Job.Worker, Job.Worker, Job.Worker});
            }
            else if (educationRan <= 90)
            {
                education = Education.Middle;

                job = getRandomElement(new Job[] { Job.Employee, Job.Officer });
            }
            else
            {
                education = Education.Academic;

                job = getRandomElement(new Job[] { Job.Capitalist, Job.Officer });
            }
        }

        // Disability
        if (job == Job.Soldier)
        {
            if (Random.Range(0, 5) == 0)
            {
                isWarDisabled = true;
            }
        }

        // Religion
        if ((job == Job.Employee || job == Job.Officer) && Random.Range(0, 5) == 0)
        {
            religion = Religion.Jewish;
        } 
        else if (Random.Range(0, 50) == 0)
        {
            religion = Religion.Atheist;
        }
        else if (Random.Range(0, 2) == 0)
        {
            religion = Religion.Protestant;
        }
        else
        {
            religion = Religion.Catholic;
        }

        // Civil Status

        if (gender == Gender.Female && Random.Range(0, 10) == 0)
        {
            civilStatus = CivilStatus.Widowed;            
        }
        else if (gender == Gender.Male && Random.Range(0,20) == 0)
        {
            civilStatus = CivilStatus.Widowed;
        }
        else if (age >= 80 && Random.Range(0,2)==0)
        {
            civilStatus = CivilStatus.Widowed;
        }
        else if (Random.Range(0,80) < age + 30)
        {
            civilStatus = CivilStatus.Married;
        }
        else
        {
            civilStatus = CivilStatus.Single;
        }

        if (civilStatus == CivilStatus.Widowed && gender == Gender.Female)
        {
            job = Job.Widow;
        }

        // Political Stance
        switch (job)
        {
            case Job.Worker:
                politicalStance = new float[] { 0, 5} [Random.Range(0, 2)];
                break;
            case Job.Employee:
                politicalStance = Random.Range(3, 5);
                break;
            case Job.Unemployed:
                politicalStance = new float[] { 0, 5 }[Random.Range(0, 2)];
                break;
            case Job.Officer:
                politicalStance = Random.Range(4, 5);
                break;
            case Job.Farmer:
                politicalStance = Random.Range(5, 6);
                break;
            case Job.Capitalist:
                politicalStance = Random.Range(5, 6);
                break;
            case Job.Soldier:
                politicalStance = Random.Range(5, 6);
                break;
            case Job.Housewive:
                politicalStance = Random.Range(3, 5);
                break;
            case Job.Widow:
                politicalStance = new float[] { 2, 5 }[Random.Range(0, 2)];
                break;
        }
        if (religion == Religion.Atheist || religion == Religion.Jewish)
        {
            politicalStance--;
        }
        else
        {
            politicalStance++;
        }

        //add +/-1 for some randomness
        politicalStance += new float[] { -1, 1 }[Random.Range(0, 2)];

        // clamp
        politicalStance = Mathf.Clamp(politicalStance, 0, 7);


        // Parties
        List<Party> partyAffiliations = new List<Party>();
        foreach(Party party in partyArray)
        {
            if(Mathf.Abs(politicalStance - (int)party) <= 1)
            {
                partyAffiliations.Add(party);
            }
        }
        partyAffiliationsArray = partyAffiliations.ToArray<Party>();
    }

    private void generateCharacterAppearance()
    {
        int jobInt = (int) job;
        if (gender == Gender.Female)
        {
            outlineSprite.sprite = bodySprite.sprite = characterSpritesFemale[jobInt];
        }
        else if (gender == Gender.Male)
        {
            outlineSprite.sprite = bodySprite.sprite = characterSpritesMale[jobInt];
        }
        bodySprite.color = Color.white;
        outlineSprite.color = Color.black;

        if (isWarDisabled)
        {
            accessoireSprite.sprite = accessoireSprites[0];
        }
        if (religion == Religion.Catholic)
        {
            accessoireSprite.sprite = accessoireSprites[1];
        }
        if (religion == Religion.Protestant)
        {
            accessoireSprite.sprite = accessoireSprites[2];
        }
    }

    void tryInteractingWithPlayer()
    {
        distFromPlayer = Vector2.Distance(transform.position, player.transform.position);
        if (distFromPlayer <= reachOfPlayer && (!playerScript.isTalkingToWalker || playerScript.talkingWaker == gameObject))
        {
            agent.isStopped = true;
            outlineSprite.enabled = true;
            playerScript.isTalkingToWalker = true;
            playerScript.talkingWaker = gameObject;
        }
        else
        {
            if(playerScript.talkingWaker == gameObject)
            {
                playerScript.isTalkingToWalker = false;
                playerScript.talkingWaker = null;
            }
            agent.isStopped = false;
            outlineSprite.enabled = false;
            speechBubbleText.text = "";
            speechBubbleSprite.enabled = false;
        }
    }

    public void respondToPlayer(Leaflets.Leaflet currentLeaflet)
    {
        switch (interactionState)
        {
            case InteractionState.None:
                speechBubbleSprite.enabled = true;
                greetPlayer();
                interactionState = InteractionState.Greeted;
                break;
            case InteractionState.Greeted:
                speechBubbleSprite.enabled = false;
                rewardLeaflet(currentLeaflet);
                interactionState = InteractionState.OwningLeaflet;
                break;
            case InteractionState.OwningLeaflet:
                speechBubbleSprite.enabled = true;
                respondAlreadyReceivedLeaflet();
                break;
        }

    }

    public void greetPlayer()
    {
        int numOfGreetings = greetings.greetings.Length;
        GreetingText[] applicableGreetings = (from g in greetings.greetings where g.job == job.ToString() select g).ToArray();
        string greeting;

        if (applicableGreetings.Length > 0)
        {
            int ranIndex = Random.Range(0, applicableGreetings.Length);
            if(Random.Range(0, 2) == 0)
            {
                greeting = applicableGreetings[ranIndex].alternative1;
            }
            else
            {
                greeting = applicableGreetings[ranIndex].alternative2;
            }
        }
        else
        {
            greeting = "Guten Tag.";
        }

        greeting = firstName + " " + "(" + age + "): " + "»" + greeting + "«";
        Debug.Log(greeting);
        speechBubbleText.text = greeting;
    }
    public void rewardLeaflet(Leaflets.Leaflet currentLeaflet)
    {
        // TODO kriegsversehrt hineinnehmen? -> Flugblätter taggen

        float politicalStance_leaf = 0f; //TODO!!


            float factor = 0f;
            if (isInArray(gender,currentLeaflet.Genders))
            {
                factor ++;
            }
            else
            {
                factor -= 2;
            }


            if (isInArray(job, currentLeaflet.Jobs))
            {
                factor ++;
            }
            else
            {
                factor--;
            }


            if (isInArray(religion, currentLeaflet.Religions))
            {
                factor ++;
            }
            else
            {
                factor -= 2;
            }


            if (isInArray(civilStatus, currentLeaflet.CivilStatuses))
            {
                factor ++;
            }



            IEnumerable<Party> intersectingParties = partyAffiliationsArray.Intersect(currentLeaflet.Parties);
            int numOfIntersectingParties = intersectingParties.Count();
            factor += numOfIntersectingParties;

            interactionState = InteractionState.OwningLeaflet;

        speechBubbleText.text = "";
            Debug.Log(Mathf.Max(0, (int)factor * 10));
            playerScript.applyReward(Mathf.Max(0, (int)factor * 10));
        
        playerScript.applyReward(0);
    }

    void respondAlreadyReceivedLeaflet()
    {
        speechBubbleText.text = "Ich habe schon ein Flugblatt bekommen!";
        Debug.Log("Ich habe schon ein Flugblatt bekommen!");
    }

    static T getRandomElement<T>(T[] tarray)
    {
        return tarray[Random.Range(0, tarray.Length)];
    }

    static bool isInArray<T>(T element, T[] array)
    {
        foreach (T el in array)
        {
            if(el.Equals(element))
            {
                return true;
            }
        }

        return false;
    }

   
}
