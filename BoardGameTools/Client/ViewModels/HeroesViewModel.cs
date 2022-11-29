﻿using BoardGameTools.Client.Models;
using BoardGameTools.Shared.Models;
using System.Net.Http.Json;
using BoardGameTools.Client.ViewModels.Interfaces;

namespace BoardGameTools.Client.ViewModels
{

    public class HeroesViewModel : IHeroesViewModel
    {
        public List<CardModel> SelectedCards { get; set; } = new();
        public string WarningMessage { get; set; } = string.Empty;

        private readonly HttpClient httpClient;
        private readonly ICharacteristicViewModel characteristicViewModel;

        public HeroesViewModel(HttpClient httpClient, ICharacteristicViewModel characteristicViewModel)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            this.characteristicViewModel = characteristicViewModel;
        }

        public async Task<List<CardModel>> GetCard()
        {
            var cards = await httpClient.GetFromJsonAsync<List<Card>>("api/card") ?? new();
            var characteristics = await characteristicViewModel.GetCharacteristic();

            return cards.Select(card =>
            {
                return CardModel.Transform(card, characteristics);
            }).ToList();
        }

        public void AddCard(CardModel card)
        {
            if (SelectedCards.Count >= 5)
                WarningMessage = "Vous ne pouvez pas jouer plus de 5 cartes";
            else
                SelectedCards.Add(card);
        }

        public void RemoveCard(CardModel card) => SelectedCards.Remove(card);
    }
}
