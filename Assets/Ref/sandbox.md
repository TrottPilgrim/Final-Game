# Todo
- rework the player movement code - it should no longer swap tiles by moving
  - instead, I'm thinking about a 2 x 2 "holder" that allows you to move tiles in a clockwise or a counterclockwise direction with z or w
    - Possibly other ways too? embiggening / debiggening the holder and moving only the outer edge of tiles.
  - Make it so the movement code does not use Axis- it's not elegant and it's just very clumsy
- Rework the logic for movement + finding matches + not doing them while lerping. Right now it is a bunch of if else statements
- Add some JUICE
  - Tiles sliding
  - Bouncing before exploding

- Add some flavor
  - Sound
    - WOOSH
    - Ding!
    - bleep
  - Art

greenvwbeetle
https://freesound.org/people/greenvwbeetle/sounds/244657/



```c#
using UnityEngine;
void Update () {
    if (Input.GetKeyDown(KeyCode.W)||Input.GetKeyDown(KeyCode.UpArrow))
    {
        
    }
    if (Input.GetKeyDown(KeyCode.S)||Input.GetKeyDown(KeyCode.DownArrow))
    {
        
    }
    if (Input.GetKeyDown(KeyCode.A)||Input.GetKeyDown(KeyCode.LeftArrow))
    {
        
    }
    if (Input.GetKeyDown(KeyCode.D)||Input.GetKeyDown(KeyCode.RightArrow))
    {
        
    }
}

```


```c#
//tilescript.cs
//Rotation
bool isRustle;
float timer;

void FixedUpdate() 
{
  timer += Time.deltaTime;
  if (!isRustle)
}


void RustleLeaves() 
{
  isRustle = true;
  
  {
    this.transform.Rotate(0, 0, Mathf.sin(timer) * 0.1f, Space.Self);
  }
}
```

```c#
using UnityEngine;
public class example : Monobehavior
{

  void Movement()
  {
    Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
    //Vector2 mouseVec2 = new Vector2(mouseRay.origin.x, mouseRay.origin.y);

    RaycastHit2D rayhitUp = Physics2D.Raycast(mouseRay.origin, Vector2.up);
    RaycastHit2D rayhitRight = Physics2D.Raycast(mouseRay.origin, Vector2.right);

  }
}
```