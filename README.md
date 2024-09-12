Veritabanini olusturduktan sonra SeedData olustur. SeedDatayi program.cs icerisinde var app = builder.Build(); den sonra tanimla.
SeedData güncellendikten sonra database drop ile kaldirilir
dotnet ef database drop --force --context BlogContext.

Bu projede MVC sablonu degil WEB sablonu kullanildi. O yüzden controller i olusturduktan sonr abizim view de olusturmamiz gerek. Program.cs te build edilmeden once gidip builder.Services.AddControllersWithViews();
ekledik..
Ve app.MapDefaultControllerRoute() ekledik. 

ViewImports klasöri ekle. Import klasorunde @isareti global anlama geliyor. View icinde @model List<> dedigimizde erismek icin.

private readonly BlogContext _context; 
Bu güvenli bir yapi degil. cünkü context ile biz _context.Tag veya _context.User lari da aliyoruz. Bu istefigimiz bir sey degil o yüzden Repository olusturuoyruz.

IQueryable yapisini  verileri listelemek icin kullaniyoruz.

Bu r epository ler de bir kapsam old icin progrsm.cs te builden once belirledik
builder.Services.AddScoped<IPostRepository,EfPostRepository>();
 

VIEW icine layout tanimladik
ViewsTART tanimladik.

Bootstrap i dahil ettik.
Program cs e de app.UseStaticFiles();
 tanimlayarak bootstrap ozelliklerini kullanaibiliyorum

libman install bootstrap-icons@1.10.5 -d wwwroot/lib/bootstrap/icons ile iconlari tanimladik. Bu kütüphaneyi de layout un icinde projeye dahil edicez.

------




