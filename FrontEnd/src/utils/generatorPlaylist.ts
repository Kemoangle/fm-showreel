import { IVideos } from '@/model/generatorPlaylist';

export class generatorPlaylist {
    createVideo = (data: IVideos) => {
        const newArr = [];
        for (let i = 0; i < data.loop; i++) {
            newArr.push(data);
        }
        return newArr;
    };

    checkCategoriesCloselyTogether = (listVideo: IVideos[]) => {
        const indexVideo = listVideo.findIndex((x, i) => {
            if (i == listVideo.length - 1) {
                return x.category == listVideo[0].category && x.category && listVideo[0].category;
            } else {
                return (
                    x.category == listVideo[i + 1].category &&
                    x.category &&
                    listVideo[i + 1].category
                );
            }
        });
        if (indexVideo >= 0) {
            return true;
        }
        return false;
    };

    checkNoBackToBack = (listVideo: IVideos[]) => {
        for (let i = 0; i < listVideo.length; i++) {
            const element = listVideo[i];
            if (element.restriction) {
                if (element.restriction.includes('No Back to Back With ')) {
                    const btbName = element.restriction.split(' With ')[1];
                    switch (btbName) {
                        case 'FMSG Contents':
                            if (
                                listVideo[i - 1].name == 'FMSG Hiring' ||
                                listVideo[i + 1].name == 'FMSG Hiring'
                            ) {
                                return true;
                            }
                            break;

                        default:
                            break;
                    }
                }
            }
        }
        return false;
    };

    handleCategoriesCloselyTogether = (listVideo: IVideos[]) => {
        const indexVideo = listVideo.findIndex((x, i) => {
            if (i == listVideo.length - 1) {
                return x.category == listVideo[0].category && x.category && listVideo[0].category;
            } else {
                return (
                    x.category == listVideo[i + 1].category &&
                    x.category &&
                    listVideo[i + 1].category
                );
            }
        });

        if (indexVideo >= 0) {
            const indexSwap = listVideo.findIndex((x, i) => {
                if (i == 0) {
                    return (
                        x.category !== listVideo[indexVideo].category &&
                        listVideo[indexVideo].category !== listVideo[i + 1].category &&
                        listVideo[indexVideo].category !== listVideo[listVideo.length - 1].category
                    );
                } else {
                    return (
                        x.category !== listVideo[indexVideo].category &&
                        listVideo[indexVideo].category !== listVideo[i + 1].category &&
                        listVideo[indexVideo].category !== listVideo[i - 1].category
                    );
                }
            });

            const newListVideo = [...listVideo];
            let swapVideo: IVideos = newListVideo[indexVideo];
            newListVideo[indexVideo] = newListVideo[indexSwap];
            newListVideo[indexSwap] = swapVideo;

            return newListVideo;
        }
    };

    handleCheckAndSortCategoriesCloselyTogether = (listVideo: IVideos[]) => {
        let isCategoriesCloselyTogether = this.checkCategoriesCloselyTogether(listVideo);

        let newListVideo: IVideos = [];
        do {
            newListVideo = this.handleCategoriesCloselyTogether(
                newListVideo.length ? newListVideo : listVideo
            );
            isCategoriesCloselyTogether = this.checkCategoriesCloselyTogether(newListVideo);
        } while (isCategoriesCloselyTogether);

        return newListVideo;
    };

    handleCustomeVideo = (listVideo: IVideos[]) => {
        const newList: IVideos = [];
        listVideo.forEach((video) => {
            newList.push(video);
        });
        return newList;
    };

    handleRemoveVideos = (listVideo: IVideos[]) => {
        const newList: IVideos = [];
        listVideo.forEach((video) => {
            if (video.loop > 1) {
                newList.push({ ...video, loop: video.loop - 1 });
            }
        });
        return newList;
    };

    createListVideo = (listVideo: IVideos[]) => {
        let oldListVideo = [...listVideo];

        oldListVideo.sort((a, b) => b.loop - a.loop);

        const newListVideo = [];

        do {
            newListVideo.push(...this.handleCustomeVideo(oldListVideo));
            oldListVideo = this.handleRemoveVideos(oldListVideo);
        } while (oldListVideo.length);

        return newListVideo;
    };
}
