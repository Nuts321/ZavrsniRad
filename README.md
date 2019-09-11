# ZavrsniRad

Pokrenete aplikaciju i registrujete se kao admin@gmai.com
Idete u HomeController i nadjete Admin sistema
//AdminSistema
        [Authorize(Roles ="Admin")]
        //[AllowAnonymous]
        public async Task<IActionResult> AdminSistema()
        
       **comment Authorize i uncomment allowanonymouse
      
Pokrenete applikaciju i ukucate /Home/AdminSistema
Zatvorite aplikaciju i vratite autorizaciju

mozete koristiti aplikaciju
