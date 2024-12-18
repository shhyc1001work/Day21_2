using System;
using System.Collections.Generic;
using System.Threading;

class Program
{
    // Item 구조체 정의
    struct Item
    {
        public string Name;
        public int Type; // 0: 무기, 1: 방어구, 2: 악세사리
        public int Attack;
        public int Defence;
        public int HP;
        public int Price;
    }

    // 전역 변수
    static int PlayerMoney = 10000;
    static List<Item> PlayerInventory = new List<Item>();
    static List<Item> PlayerEquipment = new List<Item>();
    static List<Item> ShopItems = new List<Item>();

    static void Main()
    {
        // 초기 데이터 설정
        InitializeShopItems();

        // 메인 루프
        while (true)
        {
            Console.Clear();
            int selectedMenu = MainSelect();

            switch (selectedMenu)
            {
                case 0:
                    EquipMenu();
                    break;
                case 1:
                    InventoryMenu();
                    break;
                case 2:
                    ShopMenu();
                    break;
                case 3:
                    Environment.Exit(0);
                    break;
            }
        }
    }

    static int MainSelect()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("       [상점 구현 프로그램]");
            Console.WriteLine();
            Console.WriteLine("   해당하는 숫자의 키를 누르시오");
            Console.WriteLine();
            Console.WriteLine("1. 장비창  2. 인벤토리  3. 상점  4. 종료");

            var input = Console.ReadKey(true).KeyChar;

            if (input >= '1' && input <= '4')
            {
                return input - '1'; // '1' -> 0, '2' -> 1, ...
            }

            Console.WriteLine("1, 2, 3, 4만 누르십시오");
            Thread.Sleep(500);
        }
    }

    static void EquipMenu()
    {
        while (true)
        {
            Console.Clear();

            if (PlayerEquipment.Count == 0)
            {
                Console.WriteLine("현재 장착중인 장비 없음");
            }
            else
            {
                Console.WriteLine("현재 장착중인 장비:");
                int additionalAttack = 0, additionalDefence = 0, additionalHealth = 0;

                for (int i = 0; i < PlayerEquipment.Count; i++)
                {
                    Console.WriteLine($"[{i + 1}]. {PlayerEquipment[i].Name}");

                    switch (PlayerEquipment[i].Type)
                    {
                        case 0:
                            additionalAttack += PlayerEquipment[i].Attack;
                            break;
                        case 1:
                            additionalDefence += PlayerEquipment[i].Defence;
                            break;
                        case 2:
                            additionalHealth += PlayerEquipment[i].HP;
                            break;
                    }
                }

                Console.WriteLine($"\n[장비로 인한 추가 스탯] 공: {additionalAttack} 방: {additionalDefence} 체: {additionalHealth}");
            }

            Console.WriteLine("\n1. 장착  2. 탈착  3. 메인화면");

            var input = Console.ReadKey(true).KeyChar;

            switch (input)
            {
                case '1':
                    EquipItem();
                    break;
                case '2':
                    UnequipItem();
                    break;
                case '3':
                    return;
                default:
                    Console.WriteLine("1, 2, 3만 누르십시오");
                    
                    break;
            }
        }
    }

    static void InventoryMenu()
    {
        while (true)
        {
            Console.Clear();

            if (PlayerInventory.Count == 0)
            {
                Console.WriteLine("인벤토리는 비어있습니다.");
            }
            else
            {
                ShowInventory();
            }

            Console.WriteLine($"\n보유 금액: {PlayerMoney}원");
            Console.WriteLine("\n버릴 아이템 번호를 입력하세요. (메인 메뉴로 가려면 0 입력)");

            if (int.TryParse(Console.ReadLine(), out int input))
            {
                if (input == 0)
                {
                    return;
                }

                if (input > 0 && input <= PlayerInventory.Count)
                {
                    Console.WriteLine($"[{PlayerInventory[input - 1].Name}] 아이템을 버렸습니다.");
                    PlayerInventory.RemoveAt(input - 1);
                    
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.");
                    
                }
            }
        }
    }

    static void ShopMenu()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("상점 아이템 목록:");

            for (int i = 0; i < ShopItems.Count; i++)
            {
                Console.WriteLine($"[{i + 1}]. {ShopItems[i].Name} - {ShopItems[i].Price}원");
            }

            Console.WriteLine("\n구매할 아이템 번호를 입력하세요. (메인 메뉴로 가려면 0 입력)");

            if (int.TryParse(Console.ReadLine(), out int input))
            {
                if (input == 0)
                {
                    return;
                }

                if (input > 0 && input <= ShopItems.Count)
                {
                    Item selectedItem = ShopItems[input - 1];

                    if (PlayerMoney >= selectedItem.Price)
                    {
                        PlayerInventory.Add(selectedItem);
                        PlayerMoney -= selectedItem.Price;
                        Console.WriteLine($"[{selectedItem.Name}]을(를) 구매했습니다!");
                        
                    }
                    else
                    {
                        Console.WriteLine("소지금이 부족합니다.");
                        
                    }
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.");
                    Thread.Sleep(500);
                }
            }
        }
    }

    static void EquipItem()
    {
        if (PlayerInventory.Count == 0)
        {
            Console.WriteLine("인벤토리가 비어있습니다.");
            Thread.Sleep(500);
            return;
        }

        ShowInventory();

        Console.WriteLine("\n장착할 아이템 번호를 입력하세요:");

        if (int.TryParse(Console.ReadLine(), out int input) && input > 0 && input <= PlayerInventory.Count)
        {
            Item selectedItem = PlayerInventory[input - 1];

            foreach (var equip in PlayerEquipment)
            {
                if (equip.Type == selectedItem.Type)
                {
                    Console.WriteLine("해당 부위에 이미 장비가 장착되어 있습니다.");
                    Thread.Sleep(500);
                    return;
                }
            }

            PlayerEquipment.Add(selectedItem);
            PlayerInventory.RemoveAt(input - 1);
            Console.WriteLine($"[{selectedItem.Name}] 장착 완료!");
            
        }
        else
        {
            Console.WriteLine("잘못된 입력입니다.");
            
        }
    }

    static void UnequipItem()
    {
        if (PlayerEquipment.Count == 0)
        {
            Console.WriteLine("장착된 장비가 없습니다.");
            return;
        }

        Console.WriteLine("현재 장착된 장비:");
        for (int i = 0; i < PlayerEquipment.Count; i++)
        {
            Console.WriteLine($"[{i + 1}]. {PlayerEquipment[i].Name}");
        }

        Console.WriteLine("\n탈착할 아이템 번호를 입력하세요:");

        if (int.TryParse(Console.ReadLine(), out int input) && input > 0 && input <= PlayerEquipment.Count)
        {
            Item selectedItem = PlayerEquipment[input - 1];
            PlayerInventory.Add(selectedItem);
            PlayerEquipment.RemoveAt(input - 1);
            Console.WriteLine($"[{selectedItem.Name}] 탈착 완료!");
        }
        else
        {
            Console.WriteLine("잘못된 입력입니다.");
        }
    }

    static void ShowInventory()
    {
        Console.WriteLine("현재 인벤토리:");
        for (int i = 0; i < PlayerInventory.Count; i++)
        {
            Console.WriteLine($"[{i + 1}]. {PlayerInventory[i].Name}");
        }
    }

    static void InitializeShopItems()
    {
        ShopItems.Add(new Item { Name = "무기1", Type = 0, Attack = 10, Defence = 0, HP = 0, Price = 500 });
        ShopItems.Add(new Item { Name = "방어구1", Type = 1, Attack = 0, Defence = 5, HP = 0, Price = 300 });
        ShopItems.Add(new Item { Name = "악세사리1", Type = 2, Attack = 0, Defence = 0, HP = 20, Price = 200 });
        // 추가 아이템 설정 가능
    }
}
