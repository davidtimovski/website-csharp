(function () {
    class BlogViewModel {
        constructor() {
            const self = this;

            self.decodeHtmlEntities = string => {
                const textArea = document.createElement('textarea');
                textArea.innerHTML = string;
                return textArea.value;
            };

            self.previousPostId = ko.observable(window.previousPostId);
            self.previousPostTitle = ko.observable(self.decodeHtmlEntities(window.previousPostTitle));
            self.nextPostId = ko.observable(window.nextPostId);
            self.nextPostTitle = ko.observable(self.decodeHtmlEntities(window.nextPostTitle));

            self.previousPostUrl = ko.pureComputed(() => {
                if (self.previousPostId() !== '') {
                    return '/blog/' + self.previousPostId() + '/' + self.titleToUrlFriendlyTitle(self.previousPostTitle());
                }
            });
            self.nextPostUrl = ko.pureComputed(() => {
                if (self.nextPostId() !== '') {
                    return '/blog/' + self.nextPostId() + '/' + self.titleToUrlFriendlyTitle(self.nextPostTitle());
                }
            });

            self.titleToUrlFriendlyTitle = title => {
                title = title.toLowerCase();
                let urlFriendlyTitle = '';

                for (let i = 0; i < title.length; i++) {
                    switch (title[i]) {
                        case '.':
                        case ',':
                        case ':':
                        case ';':
                        case '\'':
                        case '"':
                        case '?':
                        case '!':
                        case '~':
                        case '`':
                        case '$':
                        case '%':
                        case '^':
                        case '*':
                        case '(':
                        case ')':
                        case '[':
                        case ']':
                        case '{':
                        case '}':
                        case '/':
                        case '\\':
                        case '<':
                        case '>':
                            break;
                        case '#':
                            if (title[i - 1] && title[i - 1].toLowerCase() === 'c') {
                                urlFriendlyTitle += 'sharp';
                            }
                            break;
                        case ' ':
                            urlFriendlyTitle += '-';
                            break;
                        default:
                            urlFriendlyTitle += title[i];
                    }
                }
                return urlFriendlyTitle;
            };
        }
    }

    const blogPageResult = document.querySelectorAll('.blog-page');
    if (blogPageResult.length) {
        ko.applyBindings(new BlogViewModel(), blogPageResult[0]);
    }
})();