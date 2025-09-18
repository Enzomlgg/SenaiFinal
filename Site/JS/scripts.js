// scripts.js

document.addEventListener("DOMContentLoaded", () => {
  const btnFavs = document.querySelectorAll(".btn-fav");

  // Função para pegar favoritos do localStorage (retorna array)
  function getFavoritos() {
    return JSON.parse(localStorage.getItem("favoritos")) || [];
  }

  // Função para salvar favoritos no localStorage
  function saveFavoritos(favoritos) {
    localStorage.setItem("favoritos", JSON.stringify(favoritos));
  }

  // Atualiza o estado visual dos botões no carregamento da página
  function atualizarVisualFavoritos() {
    const favoritos = getFavoritos();
    btnFavs.forEach(btn => {
      const id = btn.dataset.id;
      if (favoritos.includes(id)) {
        btn.classList.add("favorito-ativo");
        btn.querySelector("i").classList.remove("bi-heart");
        btn.querySelector("i").classList.add("bi-heart-fill", "text-danger");
      } else {
        btn.classList.remove("favorito-ativo");
        btn.querySelector("i").classList.remove("bi-heart-fill", "text-danger");
        btn.querySelector("i").classList.add("bi-heart");
      }
    });
  }

  btnFavs.forEach(btn => {
    btn.addEventListener("click", () => {
      const id = btn.dataset.id;
      let favoritos = getFavoritos();

      if (favoritos.includes(id)) {
        // Remove dos favoritos
        favoritos = favoritos.filter(favId => favId !== id);
      } else {
        // Adiciona aos favoritos
        favoritos.push(id);
      }

      saveFavoritos(favoritos);
      atualizarVisualFavoritos();
    });
  });

  atualizarVisualFavoritos();
});

const produtos = [
  {
    id: "1",
    titulo: "Placa de Vídeo RTX 4060",
    descricao: "Placa de vídeo poderosa para games em alta qualidade e streamings fluídos.",
    precoAntigo: "R$ 3.499,00",
    precoAtual: "R$ 2.999,00",
    img: "img/rtx-4060.jpg",
    alt: "Placa de Vídeo RTX-4060"
  },
  {
    id: "2",
    titulo: "Processador Intel i9 13900K",
    descricao: "Desempenho extremo para jogos e multitarefas avançadas.",
    precoAntigo: "R$ 2.199,00",
    precoAtual: "R$ 1.899,00",
    img: "img/Intel-Core-i9.png",
    alt: "Processador Intel Core i9"
  },
  {
    id: "3",
    titulo: "Memória RAM 32GB DDR5",
    descricao: "Alta velocidade para gamers e profissionais exigentes.",
    precoAntigo: "R$ 849,00",
    precoAtual: "R$ 749,00",
    img: "img/memoria-ram.jpg",
    alt: "Memória RAM 32GB"
  },
  
  {
    id: "4",
    titulo: "SSD NVMe 1TB",
    descricao: "Velocidade máxima de leitura e escrita para seu PC.",
    precoAntigo: "R$ 599,00",
    precoAtual: "R$ 499,00",
    img: "img/SSD.jpg",
    alt: "SSD NVMe 1TB"
  },
  
  {
    id: "5",
    titulo: "Fonte gamer ATX Aerocool KCAS 1000W",
    descricao: "Energia confiável e potente para setups de alto desempenho.",
    precoAntigo: "R$ 1.299,90",
    precoAtual: "R$ 1.069,90",
    img: "img/fonte-aerocool.jpg",
    alt: "Fonte Aerocool"
  },
  
  {
    id: "6",
    titulo: "Water Cooler Neologic Liquid",
    descricao: "Resfriamento eficiente para manter a performance estável.",
    precoAntigo: "R$ 429,99",
    precoAtual: "R$ 322,00",
    img: "img/water-cooler.png",
    alt: "Water Cooler"
  },
  
  {
    id: "7",
    titulo: "Pasta térmica PCYes Nitrogen Pro 4g",
    descricao: "Máxima dissipação térmica para processadores e placas de vídeo.",
    precoAntigo: "R$ 15,90",
    precoAtual: "R$ 14,35 à vista",
    img: "img/pasta-termica.jpg",
    alt: "Pasta Térmica"
  },
  
  {
    id: "8",
    titulo: "Memória RAM 16GB DDR5 6000MHz",
    descricao: "Performance de ponta para jogos e produtividade.",
    precoAntigo: "R$ 749,90",
    precoAtual: "R$ 659,91 à vista",
    img: "img/memoria-ram-16gb.jpg",
    alt: "Memória RAM 16GB"
  }
  
];

// ----------CARRINHO ------------ //

document.addEventListener('DOMContentLoaded', () => {
  const botoesCarrinho = document.querySelectorAll('.btn-outline-primary');

  botoesCarrinho.forEach(botao => {
    botao.addEventListener('click', e => {
      e.preventDefault();

      const card = botao.closest('.card');

      if (!card) return;

      const id = card.querySelector('.btn-fav')?.dataset.id || Date.now();
      const nome = card.querySelector('.card-title')?.textContent.trim() || 'Produto sem nome';
      const preco = card.querySelector('.text-danger')?.textContent.trim() || 'R$ 0,00';
      const imagem = card.querySelector('img')?.src || '';

      const produto = { id, nome, preco, imagem };

      adicionarAoCarrinho(produto);
    });
  });

  if (window.location.pathname.includes('carrinho.html')) {
    exibirCarrinho();
  }
});

function adicionarAoCarrinho(produto) {
  let carrinho = JSON.parse(localStorage.getItem('carrinho')) || [];

  const jaExiste = carrinho.some(p => p.id === produto.id);

  if (!jaExiste) {
    carrinho.push(produto);
    localStorage.setItem('carrinho', JSON.stringify(carrinho));
    alert('✅ Produto adicionado ao carrinho!');
  } else {
    alert('⚠️ Produto já está no carrinho!');
  }
}

function exibirCarrinho() {
  const carrinho = JSON.parse(localStorage.getItem('carrinho')) || [];
  const container = document.querySelector('#carrinho-container');

  if (!container) return;

  if (carrinho.length === 0) {
    container.innerHTML = '<p class="text-center mt-4">Seu carrinho está vazio.</p>';
    return;
  }

  container.innerHTML = carrinho.map(item => `
    <div class="card mb-3 shadow-sm">
      <div class="row g-0">
        <div class="col-md-3">
          <img src="${item.imagem}" class="img-fluid rounded-start" alt="${item.nome}">
        </div>
        <div class="col-md-9">
          <div class="card-body">
            <h5 class="card-title">${item.nome}</h5>
            <p class="card-text text-danger fw-bold">${item.preco}</p>
            <button class="btn btn-danger btn-sm" onclick="removerDoCarrinho('${item.id}')">
              Remover
            </button>
          </div>
        </div>
      </div>
    </div>
  `).join('');
}

function removerDoCarrinho(id) {
  let carrinho = JSON.parse(localStorage.getItem('carrinho')) || [];
  carrinho = carrinho.filter(item => item.id !== id);
  localStorage.setItem('carrinho', JSON.stringify(carrinho));
  exibirCarrinho();
}

const id = card.dataset.id;
