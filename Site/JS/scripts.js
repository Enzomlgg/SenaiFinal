// carrinho.js
const container = document.getElementById("carrinho-itens");
const contador = document.getElementById("contador-carrinho");
let carrinho = JSON.parse(localStorage.getItem("carrinho")) || [];

// Atualiza visualização do carrinho
function atualizarCarrinho() {
  container.innerHTML = "";
  let total = 0;

  if (carrinho.length === 0) {
    container.innerHTML = "<p class='text-muted'>Ainda não há produtos no carrinho.</p>";
  } else {
    carrinho.forEach((produto, index) => {
      const precoNum = parseFloat(produto.preco.replace("R$","").replace(".","").replace(",","."));
      total += precoNum;

      container.innerHTML += `
        <div class="col-12 col-md-6 col-lg-4">
          <div class="card h-100 shadow-sm">
            <img src="${produto.img}" class="card-img-top" alt="${produto.nome}">
            <div class="card-body d-flex flex-column">
              <h5 class="card-title">${produto.nome}</h5>
              <p class="card-text fw-bold text-danger mb-3">${produto.preco}</p>
              <button class="btn btn-danger mt-auto" onclick="removerItem(${index})">Remover</button>
            </div>
          </div>
        </div>
      `;
    });
  }

  document.getElementById("carrinho-total").innerText = `Total: R$ ${total.toFixed(2).replace(".",",")}`;
  contador.innerText = carrinho.length;
}

// Remove item do carrinho
function removerItem(index) {
  carrinho.splice(index, 1);
  localStorage.setItem("carrinho", JSON.stringify(carrinho));
  atualizarCarrinho();
}

// Finalizar compra
document.getElementById("finalizar-compra").addEventListener("click", () => {
  if(carrinho.length === 0){
    alert("Seu carrinho está vazio!");
  } else {
    alert("Compra finalizada com sucesso! 🛒");
    localStorage.removeItem("carrinho");
    carrinho = [];
    atualizarCarrinho();
  }
});

// Função para adicionar produto ao carrinho (chamada pelos cards)
function adicionarAoCarrinho(produto) {
  carrinho.push(produto);
  localStorage.setItem("carrinho", JSON.stringify(carrinho));
  atualizarCarrinho();
}

// Inicializa
atualizarCarrinho();


// ---------- FAVORITOS -------------- // 

// Seleciona todos os botões de favorito
const favButtons = document.querySelectorAll('.btn-fav');

favButtons.forEach(button => {
  // Atualiza o ícone se já estiver favoritado
  const favs = JSON.parse(localStorage.getItem('favoritos')) || [];
  if (favs.find(p => p.id === button.dataset.id)) {
    button.classList.add('btn-danger');
    button.querySelector('i').classList.replace('bi-heart', 'bi-heart-fill');
  }

  button.addEventListener('click', () => {
    let favoritos = JSON.parse(localStorage.getItem('favoritos')) || [];
    const product = {
      id: button.dataset.id,
      name: button.dataset.name,
      price: button.dataset.price,
      img: button.dataset.img
    };

    const exists = favoritos.find(p => p.id === product.id);

    if (exists) {
      // Remove dos favoritos
      favoritos = favoritos.filter(p => p.id !== product.id);
      button.classList.remove('btn-danger');
      button.querySelector('i').classList.replace('bi-heart-fill', 'bi-heart');
    } else {
      // Adiciona aos favoritos
      favoritos.push(product);
      button.classList.add('btn-danger');
      button.querySelector('i').classList.replace('bi-heart', 'bi-heart-fill');
    }

    localStorage.setItem('favoritos', JSON.stringify(favoritos));
  });
});

// --------------------- FAVORITOS ------------------ //

document.addEventListener('DOMContentLoaded', () => {
  const container = document.getElementById('favoritos-container');
  const favoritos = JSON.parse(localStorage.getItem('favoritos')) || [];

  if (!container) return;

  if (favoritos.length === 0) {
    container.innerHTML = `<p class="text-center text-muted w-100">Você ainda não adicionou nenhum produto aos favoritos.</p>`;
    return;
  }

  favoritos.forEach(prod => {
    const card = document.createElement('div');
    card.classList.add('col-12', 'col-sm-6', 'col-md-4', 'col-lg-3');
    card.innerHTML = `
      <div class="card product-card h-100 shadow-sm border-0">
        <img src="${prod.img}" class="card-img-top" alt="${prod.nome}">
        <div class="card-body d-flex flex-column">
          <h5 class="card-title">${prod.nome}</h5>
          <p class="card-desc text-muted">${prod.desc}</p>
          <p class="card-text fw-bold text-danger mb-3">${prod.preco}</p>
          <div class="d-flex gap-2 mt-auto">
            <a href="#" class="btn btn-outline-primary flex-grow-1" onclick='adicionarAoCarrinho(${JSON.stringify(prod)})'>
              <i class="bi bi-cart-plus"></i> Carrinho
            </a>
            <button class="btn btn-danger flex-grow-1 remove-fav" data-id="${prod.id}">
              <i class="bi bi-trash"></i> Remover
            </button>
          </div>
        </div>
      </div>
    `;
    container.appendChild(card);
  });

  container.addEventListener('click', e => {
    if (e.target.closest('.remove-fav')) {
      const btn = e.target.closest('.remove-fav');
      const id = btn.dataset.id;
      const novosFavs = favoritos.filter(p => p.id != id);
      localStorage.setItem('favoritos', JSON.stringify(novosFavs));
      location.reload();
    }
  });
});

function adicionarAoCarrinho(produto) {
  let carrinho = JSON.parse(localStorage.getItem('carrinho')) || [];
  carrinho.push(produto);
  localStorage.setItem('carrinho', JSON.stringify(carrinho));
  alert(`${produto.nome} adicionado ao carrinho!`);
}

document.addEventListener('DOMContentLoaded', () => {
  const btnsFav = document.querySelectorAll('.btn-fav');

  // Lista de produtos com os dados do card
  const produtos = [
    {
      id: 1,
      nome: "Placa de Vídeo RTX 4060",
      desc: "Placa de vídeo poderosa para games em alta qualidade e streamings fluídos.",
      preco: "R$ 2.999,00",
      img: "img/rtx-4060.jpg"
    },
    {
      id: 2,
      nome: "Processador Intel i9 13900K",
      desc: "Desempenho extremo para jogos e multitarefas avançadas.",
      preco: "R$ 1.899,00",
      img: "img/Intel-Core-i9.png"
    },
    {
      id: 3,
      nome: "Memória RAM 32GB DDR5",
      desc: "Alta velocidade para gamers e profissionais exigentes.",
      preco: "R$ 749,00",
      img: "img/memoria-ram.jpg"
    },
    {
      id: 4,
      nome: "SSD NVMe 1TB",
      desc: "Velocidade máxima de leitura e escrita para seu PC.",
      preco: "R$ 499,00",
      img: "img/SSD.jpg"
    },
    {
      id: 5,
      nome: "Fonte gamer ATX Aerocool KCAS 1000W",
      desc: "Energia confiável e potente para setups de alto desempenho.",
      preco: "R$ 1.069,90",
      img: "img/fonte-aerocool.jpg"
    },
    {
      id: 6,
      nome: "Water Cooler Neologic Liquid",
      desc: "Resfriamento eficiente para manter a performance estável.",
      preco: "R$ 322,00",
      img: "img/water-cooler.png"
    },
    {
      id: 7,
      nome: "Pasta térmica PCYes Nitrogen Pro 4g",
      desc: "Máxima dissipação térmica para processadores e placas de vídeo.",
      preco: "R$ 14,35 à vista",
      img: "img/pasta-termica.jpg"
    },
    {
      id: 8,
      nome: "Memória RAM 16GB DDR5 6000MHz",
      desc: "Performance de ponta para jogos e produtividade.",
      preco: "R$ 659,91 à vista",
      img: "img/memoria-ram-16gb.jpg"
    },
  ];

  btnsFav.forEach(btn => {
    btn.addEventListener('click', () => {
      const id = parseInt(btn.dataset.id);
      const produto = produtos.find(p => p.id === id);

      let favoritos = JSON.parse(localStorage.getItem('favoritos')) || [];

      // Evita duplicados
      if (!favoritos.some(p => p.id === id)) {
        favoritos.push(produto);
        localStorage.setItem('favoritos', JSON.stringify(favoritos));
        alert(`${produto.nome} adicionado aos favoritos!`);
        window.location.href = "favoritos.html"; // redireciona para favoritos
      } else {
        alert(`${produto.nome} já está nos favoritos!`);
      }
    });
  });
});
