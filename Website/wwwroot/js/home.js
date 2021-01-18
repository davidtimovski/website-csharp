(function () {
    class ExpertiseViewModel {
        constructor(id, tech, answer, description, imageSrc, tags) {
            const self = this;

            self.id = id;
            self.tech = tech;
            self.answer = answer;
            self.description = description;
            self.imageSrc = imageSrc;
            self.tags = tags;
            self.matchedTags = ko.observableArray([]);

            self.tagsMatch = queryTags => {
                self.matchedTags.removeAll();
                let matchesFound = false;

                for (let i = 0; i < queryTags.length; i++) {
                    const queryTag = queryTags[i];

                    const matched = self.tags.filter(tag => tag.includes(queryTag));
                    if (matched.length === 0) {
                        continue;
                    }

                    for (let j = 0; j < matched.length; j++) {
                        const match = matched[j];

                        if (!self.matchedTags().includes(match)) {
                            self.matchedTags.push(match);
                        }
                    }
                    matchesFound = true;
                }
                return matchesFound;
            };
        }
    }

    class HomeViewModel {
        constructor() {
            const self = this;

            self.expertise = ko.observableArray([]);

            fetch('/api/expertise')
                .then(response => response.json())
                .then(items => {
                    for (let i = 0; i < items.length; i++) {
                        const item = items[i];

                        self.expertise.push(
                            new ExpertiseViewModel(item.id, item.tech, item.answer, item.description, item.imageUri, item.tags)
                        );
                    }
                });

            self.bannerImageFadeIn = () => {
                const bannerImageWrapResult = document.querySelectorAll('.banner-image-wrap');
                if (bannerImageWrapResult.length) {
                    Velocity(bannerImageWrapResult[0], { opacity: 1 }, { duration: 1000, easing: 'easeInQuad' });
                }
            };

            self.socialIconsSlideDown = () => {
                const socialIconsWrapResult = document.querySelectorAll('.social-icons-wrap');
                if (socialIconsWrapResult.length) {
                    Velocity(socialIconsWrapResult[0], { opacity: 1 }, { duration: 1000, easing: 'easeInQuad' });
                }
            };

            self.searchedTags = null;

            self.expertiseNotFound = ko.observable(false);
            self.foundExpertise = ko.observableArray();
            self.expertiseQuery = ko.observable('');
            self.expertiseQuery.subscribe(query => {
                if (query && query.length > 1) {
                    if (query.slice(-1) !== ' ') {
                        const tags = getTagsFromQuery(query);
                        const tagsString = tags.join();

                        if (self.searchedTags === tagsString) {
                            return;
                        } else {
                            self.searchedTags = tagsString;
                        }

                        self.expertiseNotFound(false);
                        self.foundExpertise.removeAll();

                        const result = self.expertise()
                            .filter((item) => item.tagsMatch(tags))
                            .filter((obj, pos, arr) => arr.map(mapObj => mapObj.id).indexOf(obj.id) === pos);

                        if (result.length) {
                            self.foundExpertise(result);
                        } else {
                            self.expertiseNotFound(true);
                        }
                    }
                } else {
                    self.expertiseNotFound(false);
                    self.foundExpertise.removeAll();
                    self.searchedTags = null;
                }
            });

            const expertisePlaceholderOptions = [
                'C#',
                'F#',
                '.NET Core',
                'ASP.NET',
                'Razor Pages',
                'Azure',
                'JavaScript',
                'TypeScript',
                'Angular',
                'AngularJS',
                'Aurelia',
                'CSS',
                'SASS',
                'SQL Server',
                'MySQL',
                'PostgreSQL',
                'Entity Framework',
                'Dapper.NET',
                'Git',
                'Docker',
                'Blazor',
                'Nginx',
                'WCF',
                'PWA',
                'Push Notifications',
                'Offline Web Apps',
                'Unit Testing',
                'RabbitMQ',
                'Hangfire',
                'Scrum',
                'Avalonia',
                'Powershell'
            ];
            const expertisePlaceholders = [];
            for (let i = 0; i < 3; i++) {
                const index = getRandom(0, expertisePlaceholderOptions.length);
                const randomPlaceholder = expertisePlaceholderOptions[index];
                expertisePlaceholders.push(randomPlaceholder);
                expertisePlaceholderOptions.splice(index, 1);
            }

            self.expertisePlaceholder = expertisePlaceholders.join(', ');
        }
    }

    const homeViewModel = new HomeViewModel();
    if (window.innerWidth > 890) {
        homeViewModel.bannerImageFadeIn();
        homeViewModel.socialIconsSlideDown();
    }

    const homePageResult = document.querySelectorAll('.home-page');
    if (homePageResult.length) {
        ko.applyBindings(homeViewModel, homePageResult[0]);
    }

    function getRandom(min, max) {
        return Math.floor(Math.random() * (max - min) + min);
    }

    function getTagsFromQuery(query) {
        query = query.toLowerCase();
        return query.split(',')
            .map(tag => { return tag.trim(); })
            .filter(tag => tag.length > 1);
    }
})();